using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AttributesOnSeparateLines : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0101";
        private const string Title = "Attributes separate lines";
        private const string MessageFormat = "Attributes should be on separate lines";
        private const string Description = "All attributes should be on separate lines and be wrapped in their own braces.";
        private const string Category = "Formatting";
        private static readonly string _HelpLinkUri = DiagnosticUrlBuilder.GetUrl(Title, 
            DiagnosticId);

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property, SymbolKind.NamedType, SymbolKind.Method, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            ISymbol namedTypeSymbol = context.Symbol;

            if (namedTypeSymbol.GetAttributes().Any())
            {
                IDictionary<int, AttributeData> lineDict = new Dictionary<int, AttributeData>();
                int symbolLineNo = namedTypeSymbol.Locations[0].GetLineSpan().StartLinePosition.Line;
                foreach (AttributeData attribute in namedTypeSymbol.GetAttributes())
                {
                    SyntaxReference applicationSyntaxReference = attribute.ApplicationSyntaxReference;
                    Microsoft.CodeAnalysis.Text.TextSpan textspan = applicationSyntaxReference.Span;
                    SyntaxTree syntaxTree = applicationSyntaxReference.SyntaxTree;
                    FileLinePositionSpan linespan = syntaxTree.GetLineSpan(textspan);

                    int attributeLineNo = linespan.StartLinePosition.Line;
                    if (lineDict.ContainsKey(attributeLineNo) || attributeLineNo == symbolLineNo)
                    {
                        Location location = syntaxTree.GetLocation(textspan);
                        Diagnostic diagnostic = Diagnostic.Create(_Rule, location, attribute.AttributeClass.Name);

                        context.ReportDiagnostic(diagnostic);
                    }
                    else
                    {
                        lineDict.Add(attributeLineNo, attribute);
                    }
                }
            }
        }
    }
}
