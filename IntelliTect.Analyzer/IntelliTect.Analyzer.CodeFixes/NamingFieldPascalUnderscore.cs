using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace IntelliTect.Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamingFieldPascalUnderscore))]
    [Shared]
    public class NamingFieldPascalUnderscore : CodeFixProvider
    {
        private const string Title = "Fix Naming Violation: Follow _PascalCase";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Analyzers.NamingFieldPascalUnderscore.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null)
            {
                return;
            }

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            SyntaxToken declaration = root.FindToken(diagnosticSpan.Start);

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedSolution: c => MakePascalWithUnderscore(context.Document, declaration, c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private static async Task<Solution> MakePascalWithUnderscore(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            string nameOfField = declaration.ValueText;
            string nameWithoutUnderscore = nameOfField.TrimStart('_');
            string newName = "_" + char.ToUpper(nameWithoutUnderscore.First()) + nameWithoutUnderscore.Substring(1);

            SemanticModel? semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (semanticModel is null || declaration.Parent is null)
            {
                return document.Project.Solution;
            }
            ISymbol? symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            if (symbol is null)
            {
                return document.Project.Solution;
            }
            Solution solution = document.Project.Solution;
            SymbolRenameOptions options = new()
            {
                RenameOverloads = true
            };
            return await Renamer.RenameSymbolAsync(solution: solution,
                                                   symbol: symbol,
                                                   options: options,
                                                   newName: newName,
                                                   cancellationToken: cancellationToken)
                                .ConfigureAwait(false);
        }
    }
}
