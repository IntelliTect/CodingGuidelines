using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace IntelliTect.Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TaskDelayZero))]
    [Shared]
    public class TaskDelayZero : CodeFixProvider
    {
        private const string Title = "Use Task.CompletedTask";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(Analyzers.TaskDelayZero.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null)
            {
                return;
            }

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            InvocationExpressionSyntax? invocation = root.FindToken(diagnosticSpan.Start)
                .Parent?.AncestorsAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .FirstOrDefault();
            if (invocation is null)
            {
                return;
            }

            SemanticModel? semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
            if (semanticModel is null)
            {
                return;
            }

            if (semanticModel.GetOperation(invocation, context.CancellationToken) is not IInvocationOperation invocationOperation)
            {
                return;
            }

            ExpressionSyntax? replacement = CreateReplacementExpression(invocationOperation);
            if (replacement is null)
            {
                return;
            }

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => ReplaceInvocationAsync(context.Document, invocation, replacement, c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private static ExpressionSyntax? CreateReplacementExpression(IInvocationOperation invocation)
        {
            if (!IsTaskDelayWithIntMilliseconds(invocation.TargetMethod))
            {
                return null;
            }

            IArgumentOperation? millisecondsDelayArgument = invocation.Arguments
                .FirstOrDefault(a => a.Parameter?.Name == "millisecondsDelay");
            if (millisecondsDelayArgument?.Value.ConstantValue is not { HasValue: true, Value: int millisecondsDelay }
                || millisecondsDelay != 0)
            {
                return null;
            }

            IArgumentOperation? cancellationTokenArgument = invocation.Arguments
                .FirstOrDefault(a => a.Parameter?.Name == "cancellationToken");
            if (cancellationTokenArgument is null)
            {
                return SyntaxFactory.ParseExpression("global::System.Threading.Tasks.Task.CompletedTask");
            }

            if (cancellationTokenArgument.Value.Syntax is not ExpressionSyntax cancellationTokenExpression)
            {
                return null;
            }

            if (!IsSideEffectFree(cancellationTokenArgument.Value))
            {
                return null;
            }

            string tokenExpressionText = NormalizeCancellationTokenExpression(cancellationTokenExpression);

            // Runtime behavior reference:
            // https://github.com/dotnet/runtime/blob/1acc89c305165239a5a824567a3176b6b3342790/src/libraries/System.Private.CoreLib/src/System/Threading/Tasks/Task.cs#L5907-L5911
            // Task.Delay(0, token) maps to:
            // token.IsCancellationRequested ? Task.FromCanceled(token) : Task.CompletedTask
            return SyntaxFactory.ParseExpression(
                $"({tokenExpressionText}.IsCancellationRequested ? global::System.Threading.Tasks.Task.FromCanceled({tokenExpressionText}) : global::System.Threading.Tasks.Task.CompletedTask)");
        }

        private static string NormalizeCancellationTokenExpression(ExpressionSyntax cancellationTokenExpression)
        {
            return cancellationTokenExpression.Kind() switch
            {
                SyntaxKind.DefaultLiteralExpression => "default(global::System.Threading.CancellationToken)",
                SyntaxKind.DefaultExpression => cancellationTokenExpression.WithoutTrivia().ToString() == "default"
                    ? "default(global::System.Threading.CancellationToken)"
                    : cancellationTokenExpression.WithoutTrivia().ToString(),
                _ => cancellationTokenExpression.WithoutTrivia().ToString()
            };
        }

        private static bool IsSideEffectFree(IOperation operation)
        {
            return operation switch
            {
                ILocalReferenceOperation => true,
                IParameterReferenceOperation => true,
                IDefaultValueOperation => true,
                IConversionOperation conversion => IsSideEffectFree(conversion.Operand),
                IParenthesizedOperation parenthesized => IsSideEffectFree(parenthesized.Operand),
                IPropertyReferenceOperation propertyReference
                    when propertyReference.Instance is null
                         && propertyReference.Property.Name == "None"
                         && propertyReference.Property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                         == "global::System.Threading.CancellationToken" => true,
                _ => false
            };
        }

        private static bool IsTaskDelayWithIntMilliseconds(IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Name != "Delay")
            {
                return false;
            }

            if (methodSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                != "global::System.Threading.Tasks.Task")
            {
                return false;
            }

            return methodSymbol.Parameters.Length >= 1
                && methodSymbol.Parameters[0].Name == "millisecondsDelay"
                && methodSymbol.Parameters[0].Type.SpecialType == SpecialType.System_Int32;
        }

        private static async Task<Document> ReplaceInvocationAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            ExpressionSyntax replacement,
            CancellationToken cancellationToken)
        {
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false)
                ?? throw new System.InvalidOperationException("Could not get syntax root");

            ExpressionSyntax replacementExpression = replacement
                .WithTriviaFrom(invocation)
                .WithAdditionalAnnotations(Formatter.Annotation);

            SyntaxNode newRoot = oldRoot.ReplaceNode(invocation, replacementExpression);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
