using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingPropertyPascal : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0002";
        private const string Title = "Properties PascalCase";
        private const string MessageFormat = "Properties should be PascalCase";
        private const string Description = "All properties should be in the format PascalCase";
        private const string Category = "Naming";
        private const string HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, 
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description,HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            ISymbol namedTypeSymbol = context.Symbol;

            var attributes = namedTypeSymbol.GetAttributes().AddRange(namedTypeSymbol.ContainingType.GetAttributes());
            if (attributes.Any(attribute => attribute.AttributeClass?.Name == nameof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
            {
                return;
            }

            if (char.IsUpper(namedTypeSymbol.Name.First()))
            {
                return;
            }

            if (namedTypeSymbol is IPropertySymbol property && property.IsIndexer)
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
