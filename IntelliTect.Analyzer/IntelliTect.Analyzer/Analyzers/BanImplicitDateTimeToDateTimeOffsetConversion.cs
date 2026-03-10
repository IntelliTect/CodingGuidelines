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
            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Conversion);
            context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
            context.RegisterOperationAction(AnalyzeBinaryOperation, OperationKind.Binary);
        }

        private void AnalyzeInvocation(OperationAnalysisContext context)
        {
            if (context.Operation is not IConversionOperation conversionOperation)
            {
                return;
            }

            if (conversionOperation.Conversion.IsImplicit && conversionOperation.Conversion.MethodSymbol is object && conversionOperation.Conversion.MethodSymbol.ContainingType is object)
            {
                INamedTypeSymbol containingType = conversionOperation.Conversion.MethodSymbol.ContainingType;
                INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset")
                    ?? throw new InvalidOperationException("System.DateTimeOffset type not found in compilation");
                if (SymbolEqualityComparer.Default.Equals(containingType, dateTimeOffsetType))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_Rule202, conversionOperation.Syntax.GetLocation()));
                }
            }


        }

        private void AnalyzeObjectCreation(OperationAnalysisContext context)
        {
            if (context.Operation is not IObjectCreationOperation objectCreation)
            {
                return;
            }

            INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset")
                ?? throw new InvalidOperationException("System.DateTimeOffset type not found in compilation");
            INamedTypeSymbol dateTimeType = context.Compilation.GetTypeByMetadataName("System.DateTime")
                ?? throw new InvalidOperationException("System.DateTime type not found in compilation");

            // Check if we're creating a DateTimeOffset
            if (!SymbolEqualityComparer.Default.Equals(objectCreation.Type, dateTimeOffsetType))
            {
                return;
            }

            // Check if the constructor has exactly one parameter and it's a DateTime
            if (objectCreation.Constructor?.Parameters.Length == 1)
            {
                IParameterSymbol parameter = objectCreation.Constructor.Parameters[0];
                if (SymbolEqualityComparer.Default.Equals(parameter.Type, dateTimeType))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_Rule202, objectCreation.Syntax.GetLocation()));
                }
            }
        }

        private void AnalyzeBinaryOperation(OperationAnalysisContext context)
        {
            if (context.Operation is not IBinaryOperation binaryOperation)
            {
                return;
            }

            INamedTypeSymbol dateTimeType = context.Compilation.GetTypeByMetadataName("System.DateTime")
                ?? throw new InvalidOperationException("System.DateTime type not found in compilation");
            INamedTypeSymbol dateTimeOffsetType = context.Compilation.GetTypeByMetadataName("System.DateTimeOffset")
                ?? throw new InvalidOperationException("System.DateTimeOffset type not found in compilation");

            CheckBinaryOperandPair(context, binaryOperation.LeftOperand, binaryOperation.RightOperand, dateTimeType, dateTimeOffsetType);
            CheckBinaryOperandPair(context, binaryOperation.RightOperand, binaryOperation.LeftOperand, dateTimeType, dateTimeOffsetType);
        }

        private static void CheckBinaryOperandPair(OperationAnalysisContext context, IOperation operand, IOperation otherOperand, INamedTypeSymbol dateTimeType, INamedTypeSymbol dateTimeOffsetType)
        {
            if (operand?.Type is null || otherOperand?.Type is null)
            {
                return;
            }

            if (operand.Syntax is null)
            {
                return;
            }

            // Skip if the operand is already a conversion — the conversion handler catches those
            if (operand is IConversionOperation)
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

            bool isDateTimeOperand = SymbolEqualityComparer.Default.Equals(operandType, dateTimeType);
            bool isDateTimeOffsetOtherOperand = SymbolEqualityComparer.Default.Equals(otherType, dateTimeOffsetType);

            if (isDateTimeOperand && isDateTimeOffsetOtherOperand)
            {
                context.ReportDiagnostic(Diagnostic.Create(_Rule202, operand.Syntax.GetLocation()));
            }
        }

        private static class Rule202
        {
            internal const string DiagnosticId = "INTL0202";
            internal const string Title = "Do not use implicit conversion from `DateTime` to `DateTimeOffset`";
            internal const string MessageFormat = "Using 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' or 'new DateTimeOffset(DateTime)' can result in unpredictable behavior";
#pragma warning disable INTL0001 // Allow field to not be prefixed with an underscore to match the style
            internal static readonly string HelpMessageUri = DiagnosticUrlBuilder.GetUrl(Title,
                DiagnosticId);
#pragma warning restore INTL0001 

            internal const string Description =
                "Implicit conversion of `DateTime` to `DateTimeOffset` determines timezone offset based on the `DateTime.Kind` value, and for `DateTimeKind.Unspecified` it assumes `DateTimeKind.Local`, which may lead to differing behavior between running locally and on a server.";
        }
    }
}
