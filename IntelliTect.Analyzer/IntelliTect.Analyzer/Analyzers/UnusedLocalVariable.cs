using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnusedLocalVariable : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0303";
        private const string _Title = "Local variable unused";
        private const string _MessageFormat = "Local variable '{0}' should be used";
        private const string _Description = "All local variables should be accessed, or named with underscores to indicate they are unused.";
        private const string _Category = "Flow";
        private const string _HelpLinkUri = "https://github.com/IntelliTect/CodingGuidelines";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, _Title, _MessageFormat,
            _Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: _Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax method && method.Body != null)
            {
                DataFlowAnalysis dataFlow = context.SemanticModel.AnalyzeDataFlow(method.Body);

                ImmutableArray<ISymbol> variablesDeclared = dataFlow.VariablesDeclared;
                IEnumerable<ISymbol> variablesRead = dataFlow.ReadInside.Union(dataFlow.ReadOutside);
                IEnumerable<ISymbol> unused = variablesDeclared.Except(variablesRead)
                    .Where(x => !(x.Name.All(c => c == '_')));

                foreach (ISymbol unusedVar in unused)
                {
                    context.ReportDiagnostic(Diagnostic.Create(_Rule, unusedVar.Locations.First(), unusedVar.Name));
                }
            }
        }
    }
}
