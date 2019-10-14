using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncVoid : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0201";
        private const string Title = "Async void methods";
        private const string MessageFormat = "Async methods should not return void.";
        private const string Description = "Async methods must return either Task or Task<T>.";
        private const string Category = "Design";
        private const string HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, 
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = context.Symbol as IMethodSymbol;

            if (methodSymbol.IsAsync && methodSymbol.ReturnsVoid)
            {
                context.ReportDiagnostic(Diagnostic.Create(_Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}
