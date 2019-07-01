using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using IntelliTectAnalyzer.Analyzers;

namespace IntelliTectAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamingFieldPascalUnderscore)), Shared]
    public class NamingFieldPascalUnderscore : CodeFixProvider
    {
        private const string _Title = "Fix Naming Violation: Follow _PascalCase";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Analyzers.NamingFieldPascalUnderscore._DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start);

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: _Title,
                    createChangedSolution: c => MakePascalWithUnderscore(context.Document, declaration, c),
                    equivalenceKey: _Title),
                diagnostic);
        }

        private async Task<Solution> MakePascalWithUnderscore(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            var nameOfField = declaration.ValueText;
            string nameWithoutUnderscore = nameOfField.TrimStart('_');
            var newName = "_" + char.ToUpper(nameWithoutUnderscore.First()) + nameWithoutUnderscore.Substring(1);

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            ISymbol symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            Solution solution = document.Project.Solution;
            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }
    }
}
