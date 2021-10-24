using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

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
            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Conversion);
        }

        private void AnalyzeInvocation(OperationAnalysisContext context)
        {
            if (!(context.Operation is IConversionOperation conversionOperation))
            {
                return;
            }

            if (conversionOperation.Conversion.IsImplicit)
            {
                INamedTypeSymbol containingType = conversionOperation.Conversion.MethodSymbol.ContainingType;
                INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset");
                if (containingType.Equals(dateTimeOffsetType))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_Rule304, conversionOperation.Syntax.GetLocation()));
                }
            }


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
