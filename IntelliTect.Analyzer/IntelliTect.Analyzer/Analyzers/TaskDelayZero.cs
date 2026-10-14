using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TaskDelayZero : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0304";
        private const string Title = "Use Task.CompletedTask for zero-delay task";
        private const string MessageFormat = "Replace Task.Delay(0) with Task.CompletedTask";
        private const string Description =
            "Task.Delay with a zero millisecond delay can be replaced with Task.CompletedTask.";
        private const string Category = "Performance";
        private static readonly string _HelpLinkUri = DiagnosticUrlBuilder.GetUrl(Title, DiagnosticId);

        private static readonly DiagnosticDescriptor _Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Info,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
        }

        private static void AnalyzeInvocation(OperationAnalysisContext context)
        {
            if (context.Operation is not IInvocationOperation invocation)
            {
                return;
            }

            if (!IsTaskDelayWithIntMilliseconds(invocation.TargetMethod))
            {
                return;
            }

            IArgumentOperation? millisecondsDelayArgument = invocation.Arguments
                .FirstOrDefault(a => string.Equals(a.Parameter?.Name, "millisecondsDelay", StringComparison.Ordinal));

            if (millisecondsDelayArgument?.Value.ConstantValue is { HasValue: true, Value: int millisecondsDelay }
                && millisecondsDelay == 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(_Rule, invocation.Syntax.GetLocation()));
            }
        }

        public static bool IsTaskDelayWithIntMilliseconds(IMethodSymbol methodSymbol)
        {
            if (!string.Equals(methodSymbol.Name, "Delay", StringComparison.Ordinal))
            {
                return false;
            }

            if (methodSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                != "global::System.Threading.Tasks.Task")
            {
                return false;
            }

            return methodSymbol.Parameters.Length >= 1
                && string.Equals(methodSymbol.Parameters[0].Name, "millisecondsDelay", StringComparison.Ordinal)
                && methodSymbol.Parameters[0].Type.SpecialType == SpecialType.System_Int32;
        }
    }
}
