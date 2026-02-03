using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BanImplicitDateTimeToDateTimeOffsetConversion : DiagnosticAnalyzer
    {
        private const string Category = "Reliability";

        private static readonly DiagnosticDescriptor _Rule202 = new(Rule202.DiagnosticId,
            Rule202.Title,
            Rule202.MessageFormat,
            Category, DiagnosticSeverity.Warning, true, Rule202.Description,
            Rule202.HelpMessageUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule202);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeConversion, OperationKind.Conversion);
            context.RegisterOperationAction(AnalyzeBinaryOperation, OperationKind.Binary);
        }

        private void AnalyzeConversion(OperationAnalysisContext context)
        {
            if (context.Operation is not IConversionOperation conversionOperation || !conversionOperation.Conversion.IsImplicit)
            {
                return;
            }

            ITypeSymbol sourceType = conversionOperation.Operand.Type;
            ITypeSymbol targetType = conversionOperation.Type;

            if (sourceType is null || targetType is null)
            {
                return;
            }

            INamedTypeSymbol dateTimeType = context.Compilation.GetTypeByMetadataName("System.DateTime")
                ?? throw new InvalidOperationException("System.DateTime type not found in compilation.");
            INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset")
                ?? throw new InvalidOperationException("System.DateTimeOffset type not found in compilation.");

            // Check if source is DateTime and target is DateTimeOffset
            if (SymbolEqualityComparer.Default.Equals(sourceType, dateTimeType) && 
                SymbolEqualityComparer.Default.Equals(targetType, dateTimeOffsetType))
            {
                // Report the diagnostic at the operand's location (the DateTime expression being converted)
                context.ReportDiagnostic(Diagnostic.Create(_Rule202, conversionOperation.Operand.Syntax.GetLocation()));
            }
        }

        private void AnalyzeBinaryOperation(OperationAnalysisContext context)
        {
            if (context.Operation is not IBinaryOperation binaryOperation)
            {
                return;
            }

            INamedTypeSymbol dateTimeType = context.Compilation.GetTypeByMetadataName("System.DateTime")
                ?? throw new InvalidOperationException("System.DateTime type not found in compilation.");
            INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset")
                ?? throw new InvalidOperationException("System.DateTimeOffset type not found in compilation.");

            // For binary operations, check if either operand is DateTime and the other is DateTimeOffset
            // This catches cases where IConversionOperation nodes are not created (e.g., property access)
            CheckBinaryOperandPair(context, binaryOperation.LeftOperand, binaryOperation.RightOperand, dateTimeType, dateTimeOffsetType);
            CheckBinaryOperandPair(context, binaryOperation.RightOperand, binaryOperation.LeftOperand, dateTimeType, dateTimeOffsetType);
        }

        private void CheckBinaryOperandPair(OperationAnalysisContext context, IOperation operand, IOperation otherOperand, INamedTypeSymbol dateTimeType, INamedTypeSymbol dateTimeOffsetType)
        {
            if (operand?.Type is null || otherOperand?.Type is null)
            {
                return;
            }

            // Skip if we don't have a syntax location to report
            if (operand.Syntax is null)
            {
                return;
            }

            // Unwrap nullable types if present
            ITypeSymbol operandType = operand.Type is INamedTypeSymbol { IsValueType: true, OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable
                ? nullable.TypeArguments[0]
                : operand.Type;

            ITypeSymbol otherType = otherOperand.Type is INamedTypeSymbol { IsValueType: true, OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } otherNullable
                ? otherNullable.TypeArguments[0]
                : otherOperand.Type;

            // Check if operand is DateTime and other operand is DateTimeOffset
            bool isDateTimeOperand = SymbolEqualityComparer.Default.Equals(operandType, dateTimeType);
            bool isDateTimeOffsetOtherOperand = SymbolEqualityComparer.Default.Equals(otherType, dateTimeOffsetType);

            if (isDateTimeOperand && isDateTimeOffsetOtherOperand)
            {
                // DateTime will be implicitly converted to DateTimeOffset in the comparison
                context.ReportDiagnostic(Diagnostic.Create(_Rule202, operand.Syntax.GetLocation()));
            }
        }

        private static class Rule202
        {
            internal const string DiagnosticId = "INTL0202";
            internal const string Title = "Do not use implicit conversion from `DateTime` to `DateTimeOffset`";
            internal const string MessageFormat = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior";
#pragma warning disable INTL0001 // Allow field to not be prefixed with an underscore to match the style
            internal static readonly string HelpMessageUri = DiagnosticUrlBuilder.GetUrl(Title,
                DiagnosticId);
#pragma warning restore INTL0001 

            internal const string Description =
                "Implicit conversion of `DateTime` to `DateTimeOffset` determines timezone offset based on the `DateTime.Kind` value, and for `DateTimeKind.Unspecified` it assumes `DateTimeKind.Local`, which may lead to differing behavior between running locally and on a server.";
        }
    }
}
