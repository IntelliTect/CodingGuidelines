using System;
using System.Collections.Immutable;
using System.Linq;
using IntelliTect.Analyzer.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingFieldPascalUnderscore : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0001";
        private const string Title = "Fields _PascalCase";
        private const string MessageFormat = "Field '{0}' should be named _PascalCase";
        private const string Description = "All fields should be in the format _PascalCase.";
        private const string Category = "Naming";
        private static readonly string _HelpLinkUri = DiagnosticUrlBuilder.GetUrl(Title,
            DiagnosticId);

        private static readonly DiagnosticDescriptor _Rule = new(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            ISymbol namedTypeSymbol = context.Symbol;

            // ignore GeneratedCodeAttribute on field and first containing type
            ImmutableArray<AttributeData> attributes = namedTypeSymbol.GetAttributes().AddRange(namedTypeSymbol.ContainingType.GetAttributes());
            if (attributes.Any(attribute => attribute.AttributeClass?.Name == nameof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
            {
                return;
            }

            //Enum members should not be flagged
            if (namedTypeSymbol.ContainingType.EnumUnderlyingType is not null)
            {
                return;
            }

            if (namedTypeSymbol.ContainingType.IsNativeMethodsClass())
            {
                return;
            }

            if (namedTypeSymbol.Name.StartsWith("_", StringComparison.Ordinal) && namedTypeSymbol.Name.Length > 1
                                                     && Casing.IsPascalCase(namedTypeSymbol.Name.Skip(1)))
            {
                return;
            }

            if (namedTypeSymbol is IFieldSymbol field && field.IsConst)
            {
                return;
            }

            var diagnostic = Diagnostic.Create(_Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
