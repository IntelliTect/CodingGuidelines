using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AttributesOnSeparateLines : DiagnosticAnalyzer
    {
        public const string _DiagnosticId = "INTL0003";
        private const string _Title = "Attributes separate lines";
        private const string _MessageFormat = "Attributes should be on separate lines";
        private const string _Description = "All attributes should be on separate lines and be wrapped in their own braces.";
        private const string _Category = "Formatting";
        private const string _HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(_DiagnosticId, _Title, _MessageFormat,
            _Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: _Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property, SymbolKind.NamedType, SymbolKind.Method, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            ISymbol namedTypeSymbol = context.Symbol;

            if (namedTypeSymbol.GetAttributes().Any())
            {
                IDictionary<int, AttributeData> lineDict = new Dictionary<int, AttributeData>();
                foreach (AttributeData attribute in namedTypeSymbol.GetAttributes())
                {
                    SyntaxReference applicationSyntaxReference = attribute.ApplicationSyntaxReference;
                    Microsoft.CodeAnalysis.Text.TextSpan textspan = applicationSyntaxReference.Span;
                    SyntaxTree syntaxTree = applicationSyntaxReference.SyntaxTree;
                    FileLinePositionSpan linespan = syntaxTree.GetLineSpan(textspan);

                    int lineNo = linespan.StartLinePosition.Line;
                    if (lineDict.ContainsKey(lineNo))
                    {
                        Location location = syntaxTree.GetLocation(textspan);
                        Diagnostic diagnostic = Diagnostic.Create(_Rule, location, attribute.AttributeClass.Name);

                        context.ReportDiagnostic(diagnostic);
                    }
                    else
                    {
                        lineDict.Add(lineNo, attribute);
                    }
                }
            }
        }
    }
}
