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
using Microsoft.CodeAnalysis.Text;

namespace IntelliTect.Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(FavorDirectoryEnumerationCalls))]
    [Shared]
    public class FavorDirectoryEnumerationCalls : CodeFixProvider
    {
        private const string TitleGetFiles = "Use Directory.EnumerateFiles";
        private const string TitleGetDirectories = "Use Directory.EnumerateDirectories";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(
                Analyzers.FavorDirectoryEnumerationCalls.DiagnosticId301,
                Analyzers.FavorDirectoryEnumerationCalls.DiagnosticId302);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            // The diagnostic span covers the full invocation expression (Directory.GetFiles(...))
            InvocationExpressionSyntax invocation = root.FindToken(diagnosticSpan.Start)
                .Parent.AncestorsAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .First();

            bool isGetFiles = diagnostic.Id == Analyzers.FavorDirectoryEnumerationCalls.DiagnosticId301;
            string title = isGetFiles ? TitleGetFiles : TitleGetDirectories;
            string newMethodName = isGetFiles ? "EnumerateFiles" : "EnumerateDirectories";

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => UseEnumerationMethodAsync(context.Document, invocation, newMethodName, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private static async Task<Document> UseEnumerationMethodAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            string newMethodName,
            CancellationToken cancellationToken)
        {
            var memberAccess = (MemberAccessExpressionSyntax)invocation.Expression;

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            // Rename: Directory.GetFiles(...) → Directory.EnumerateFiles(...)
            InvocationExpressionSyntax renamedInvocation = invocation.WithExpression(
                memberAccess.WithName(SyntaxFactory.IdentifierName(newMethodName)));

            ExpressionSyntax replacement = NeedsToArrayWrapper(invocation, semanticModel, cancellationToken)
                // Wrap as Directory.EnumerateFiles(...).ToArray()
                ? SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        renamedInvocation,
                        SyntaxFactory.IdentifierName("ToArray")))
                : renamedInvocation;

            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot.ReplaceNode(invocation, replacement.WithAdditionalAnnotations(Formatter.Annotation));

            if (replacement != renamedInvocation && newRoot is CompilationUnitSyntax compilationUnit)
            {
                newRoot = AddUsingIfMissing(compilationUnit, "System.Linq");
            }

            return document.WithSyntaxRoot(newRoot);
        }

        private static bool NeedsToArrayWrapper(
            InvocationExpressionSyntax invocation,
            SemanticModel semanticModel,
            CancellationToken ct)
        {
            SyntaxNode parent = invocation.Parent;

            // string[] files = Directory.GetFiles(...)  or field/property initializer
            if (parent is EqualsValueClauseSyntax equalsValue)
            {
                // Local variable or field: string[] files = ... / private string[] _files = ...
                if (equalsValue.Parent is VariableDeclaratorSyntax
                    && equalsValue.Parent.Parent is VariableDeclarationSyntax declaration
                    && semanticModel.GetTypeInfo(declaration.Type, ct).Type is IArrayTypeSymbol)
                {
                    return true;
                }

                // Property initializer: public string[] Files { get; } = Directory.GetFiles(...)
                if (equalsValue.Parent is PropertyDeclarationSyntax property
                    && semanticModel.GetTypeInfo(property.Type, ct).Type is IArrayTypeSymbol)
                {
                    return true;
                }
            }

            // files = Directory.GetFiles(...)
            if (parent is AssignmentExpressionSyntax assignment
                && semanticModel.GetTypeInfo(assignment.Left, ct).Type is IArrayTypeSymbol)
            {
                return true;
            }

            // return Directory.GetFiles(...)  in a method or local function returning string[]
            if (parent is ReturnStatementSyntax)
            {
                TypeSyntax returnType = invocation.Ancestors()
                    .Select(a => a switch
                    {
                        MethodDeclarationSyntax m => m.ReturnType,
                        LocalFunctionStatementSyntax lf => lf.ReturnType,
                        _ => null
                    })
                    .FirstOrDefault(t => t != null);
                if (returnType != null
                    && semanticModel.GetTypeInfo(returnType, ct).Type is IArrayTypeSymbol)
                {
                    return true;
                }
            }

            // Expression-bodied members: string[] GetFiles() => Directory.GetFiles(...)
            if (parent is ArrowExpressionClauseSyntax arrow)
            {
                TypeSyntax returnType = arrow.Parent switch
                {
                    MethodDeclarationSyntax m => m.ReturnType,
                    LocalFunctionStatementSyntax lf => lf.ReturnType,
                    PropertyDeclarationSyntax p => p.Type,
                    _ => null
                };
                if (returnType != null && semanticModel.GetTypeInfo(returnType, ct).Type is IArrayTypeSymbol)
                {
                    return true;
                }
            }

            // SomeMethod(Directory.GetFiles(...)) where the parameter type is string[]
            if (parent is ArgumentSyntax argument
                && argument.Parent is ArgumentListSyntax argumentList
                && argumentList.Parent is InvocationExpressionSyntax outerInvocation
                && semanticModel.GetSymbolInfo(outerInvocation, ct).Symbol is IMethodSymbol outerMethod)
            {
                IParameterSymbol targetParam;

                // Named argument: SomeMethod(param: Directory.GetFiles(...))
                if (argument.NameColon != null)
                {
                    string paramName = argument.NameColon.Name.Identifier.Text;
                    targetParam = outerMethod.Parameters.FirstOrDefault(p => p.Name == paramName);
                }
                else
                {
                    int argIndex = argumentList.Arguments.IndexOf(argument);
                    int paramCount = outerMethod.Parameters.Length;
                    targetParam = argIndex >= 0 && argIndex < paramCount
                        ? outerMethod.Parameters[argIndex]
                        : argIndex >= 0 && paramCount > 0 && outerMethod.Parameters[paramCount - 1].IsParams
                            ? outerMethod.Parameters[paramCount - 1]
                            : null;
                }

                if (targetParam?.Type is IArrayTypeSymbol)
                {
                    return true;
                }
            }

            return false;
        }

        private static SyntaxNode AddUsingIfMissing(CompilationUnitSyntax root, string namespaceName)
        {
            bool alreadyPresent = root.Usings.Any(u => u.Name?.ToString() == namespaceName);
            if (alreadyPresent)
            {
                return root;
            }

            UsingDirectiveSyntax newUsing = SyntaxFactory.UsingDirective(
                SyntaxFactory.ParseName(namespaceName))
                .NormalizeWhitespace()
                .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);

            return root.AddUsings(newUsing);
        }
    }
}
