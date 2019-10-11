using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FavorEnumerateFiles : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0100";
        private const string Title = "Favor using EnumerateFiles";
        private const string MessageFormat = "Favor using the method `EnumerateFiles` over the `GetFiles` method.";
        private const string Description = "When you use EnumerateFiles, you can start enumerating the collection of names before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array. Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.";
        private const string Category = "Performance";
        private const string HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description, HelpLinkUri);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

       

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        private static void AnalyzeSymbol(SymbolAnalysisContext obj)
        {
            throw new NotImplementedException();
        }
    }
}
