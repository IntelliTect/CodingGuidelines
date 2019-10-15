using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FavorEnumerateFiles : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0200";
        private const string Title = "Favor using EnumerateFiles";
        private const string MessageFormat = "Favor using the method `EnumerateFiles` over the `GetFiles` method.";

        private const string Description =
            "When you use EnumerateFiles, you can start enumerating the collection of names before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array. Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.";

        private const string Category = "Usage";
        private const string HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat,
            Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description, HelpLinkUri);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var expression = (InvocationExpressionSyntax)context.Node;

            if (!(expression.Expression is MemberAccessExpressionSyntax memberAccess)) 
                return;

            var nameSyntax = (IdentifierNameSyntax)memberAccess.Expression;

            if (string.Equals(nameSyntax.Identifier.Text, "Directory", StringComparison.CurrentCultureIgnoreCase) &&
                memberAccess.ChildNodes().Cast<IdentifierNameSyntax>().Any(x =>
                    string.Equals(x.Identifier.Text, "GetFiles", StringComparison.CurrentCultureIgnoreCase)))
            {
                // Unsure if this is the best way to determine if member was defined in the project.
                SymbolInfo symbol = context.SemanticModel.GetSymbolInfo(nameSyntax);
                if (symbol.Symbol == null)
                {
                    Location loc = memberAccess.GetLocation();
                    context.ReportDiagnostic(Diagnostic.Create(_Rule, loc, memberAccess.Name));
                }
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

    }
}
