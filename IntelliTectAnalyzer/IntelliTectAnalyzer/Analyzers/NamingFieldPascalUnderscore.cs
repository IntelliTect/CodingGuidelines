using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingFieldPascalUnderscore : DiagnosticAnalyzer
    {
        public const string _DiagnosticId = "INTL0001";
        private const string _Title = "Fields _PascalCase";
        private const string _MessageFormat = "Fields should be named _PascalCase";
        private const string _Description = "All fields should be in the format _PascalCase";
        private const string _Category = "Naming";
        private const string _HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(_DiagnosticId, _Title, _MessageFormat, 
            _Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: _Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol;

            if (namedTypeSymbol.Name.StartsWith("_") && namedTypeSymbol.Name.Length > 1 
                                                     && char.IsUpper(namedTypeSymbol.Name.Skip(1).First()))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
