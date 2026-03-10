using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace IntelliTect.Analyzer
{
    internal static class SymbolMixins
    {
        public static bool IsNativeMethodsClass(this ISymbol type)
        {
            return type.Kind == SymbolKind.NamedType && string.Equals(type.Name, "NativeMethods", StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the symbol or its containing type has
        /// <see cref="System.CodeDom.Compiler.GeneratedCodeAttribute"/>.
        /// </summary>
        public static bool HasGeneratedCodeAttribute(this ISymbol symbol, Compilation compilation)
        {
            INamedTypeSymbol? generatedCodeAttribute = compilation
                .GetTypeByMetadataName("System.CodeDom.Compiler.GeneratedCodeAttribute");
            if (generatedCodeAttribute is null) return false;

            ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
            if (symbol.ContainingType is not null)
            {
                attributes = attributes.AddRange(symbol.ContainingType.GetAttributes());
            }
            return attributes.Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, generatedCodeAttribute));
        }
    }
}
