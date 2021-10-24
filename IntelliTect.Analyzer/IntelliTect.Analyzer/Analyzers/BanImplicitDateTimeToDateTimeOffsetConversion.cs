using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BanImplicitDateTimeToDateTimeOffsetConversion : DiagnosticAnalyzer
    {
        private const string Category = "Performance";

        private static readonly DiagnosticDescriptor _Rule304 = new DiagnosticDescriptor(Rule304.DiagnosticId,
            Rule304.Title,
            Rule304.MessageFormat,
            Category, DiagnosticSeverity.Warning, true, Rule304.Description,
            Rule304.HelpMessageUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule304);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            //context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static class Rule304
        {
            internal const string DiagnosticId = "INTL0304";
            internal const string Title = "Do not use implicit conversion between `DateTime` and `DateTimeOffset`";
            internal const string MessageFormat = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior.";
#pragma warning disable INTL0001 // Allow field to not be prefixed with an underscore to match the style
            internal static readonly string HelpMessageUri = DiagnosticUrlBuilder.GetUrl(Title,
                DiagnosticId);
#pragma warning restore INTL0001 

            internal const string Description =
                "When you use EnumerateFiles, you can start enumerating the collection of names before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array. Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.";
        }
    }
}
