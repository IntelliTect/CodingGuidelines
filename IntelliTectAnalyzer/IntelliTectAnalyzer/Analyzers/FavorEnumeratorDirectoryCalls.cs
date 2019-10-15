using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FavorEnumeratorDirectoryCalls : DiagnosticAnalyzer
    {
        private const string Category = "Usage";
        private const string HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";


        private static readonly DiagnosticDescriptor _Rule300 = new DiagnosticDescriptor(Rule300.DiagnosticId,
            Rule300.Title,
            Rule300.MessageFormat,
            Category, DiagnosticSeverity.Info, true, Rule300.Description, HelpLinkUri);

        private static readonly DiagnosticDescriptor _Rule301 = new DiagnosticDescriptor(Rule301.DiagnosticId,
            Rule301.Title,
            Rule301.MessageFormat,
            Category, DiagnosticSeverity.Info, true, Rule301.Description, HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(_Rule300, _Rule301);

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

            if (string.Equals(nameSyntax.Identifier.Text, "Directory", StringComparison.CurrentCultureIgnoreCase))
            {
                if (memberAccess.ChildNodes().Cast<IdentifierNameSyntax>().Any(x =>
                        string.Equals(x.Identifier.Text, "GetFiles", StringComparison.CurrentCultureIgnoreCase)))
                {
                    // Unsure if this is the best way to determine if member was defined in the project.
                    SymbolInfo symbol = context.SemanticModel.GetSymbolInfo(nameSyntax);
                    if (symbol.Symbol == null)
                    {
                        Location loc = memberAccess.GetLocation();
                        context.ReportDiagnostic(Diagnostic.Create(_Rule300, loc, memberAccess.Name));
                    }
                }

                if (memberAccess.ChildNodes().Cast<IdentifierNameSyntax>().Any(x =>
                    string.Equals(x.Identifier.Text, "GetDirectories", StringComparison.CurrentCultureIgnoreCase)))
                {
                    // Unsure if this is the best way to determine if member was defined in the project.
                    SymbolInfo symbol = context.SemanticModel.GetSymbolInfo(nameSyntax);
                    if (symbol.Symbol == null)
                    {
                        Location loc = memberAccess.GetLocation();
                        context.ReportDiagnostic(Diagnostic.Create(_Rule301, loc, memberAccess.Name));
                    }
                }
            }
        }

        private static class Rule300
        {
            internal const string DiagnosticId = "INTL0300";
            internal const string Title = "Favor using EnumerateFiles";
            internal const string MessageFormat = "Favor using the method `EnumerateFiles` over the `GetFiles` method.";

            internal const string Description =
                "When you use EnumerateFiles, you can start enumerating the collection of names before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array. Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.";
        }

        private static class Rule301
        {
            internal const string DiagnosticId = "INTL0301";
            internal const string Title = "Favor using EnumerateDirectories";
            internal const string MessageFormat = "Favor using the method `EnumerateDirectories` over the `GetDirectories` method.";

            internal const string Description =
                "When you use EnumerateDirectories, you can start enumerating the collection of names before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array. Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.";
        }
    }
}
