using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnusedLocalVariable : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0303";
        private const string _Title = "Local variable unused";
        private const string _MessageFormat = "Local variables should be used";
        private const string _Description = "All local variables should be used accessed";
        private const string _Category = "Flow";
        private const string _HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

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
            if (context.Node is MethodDeclarationSyntax method)
            {
                DataFlowAnalysis dataFlow = context.SemanticModel.AnalyzeDataFlow(method.Body);

                ImmutableArray<ISymbol> variablesDeclared = dataFlow.VariablesDeclared;
                IEnumerable<ISymbol> variablesRead = dataFlow.ReadInside.Union(dataFlow.ReadOutside);
                IEnumerable<ISymbol> unused = variablesDeclared.Except(variablesRead);

                foreach (ISymbol unusedVar in unused)
                {
                    context.ReportDiagnostic(Diagnostic.Create(_Rule, unusedVar.Locations.First()));
                }
            }
        }
    }
}
