
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;
using IntelliTect.Analyzer.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingMethodPascal : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0003";
        private const string Title = "Methods PascalCase";
        private const string MessageFormat = "Method '{0}' should be PascalCase";
        private const string Description = "All methods should be in the format PascalCase.";
        private const string Category = "Naming";
        private static readonly string _HelpLinkUri = DiagnosticUrlBuilder.GetUrl(Title,
            DiagnosticId);

        private static readonly DiagnosticDescriptor _Rule = new(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, _HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.LocalFunctionStatement);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var localFunctionStatement = (LocalFunctionStatementSyntax)context.Node;

            if (Casing.IsPascalCase(localFunctionStatement.Identifier.Text))
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_Rule, localFunctionStatement.Identifier.GetLocation(), localFunctionStatement.Identifier.Text);

            context.ReportDiagnostic(diagnostic);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (IMethodSymbol)context.Symbol;

            if (namedTypeSymbol.ContainingType.IsNativeMethodsClass())
            {
                return;
            }

            string name;
            switch (namedTypeSymbol.MethodKind)
            {
                case MethodKind.DeclareMethod:
                case MethodKind.LocalFunction:
                case MethodKind.Ordinary:
                case MethodKind.ReducedExtension:
                    name = namedTypeSymbol.Name;
                    break;
                case MethodKind.ExplicitInterfaceImplementation:
                    name = namedTypeSymbol.ExplicitInterfaceImplementations.First().Name;
                    break;
                default: return;
            }

            // Common symbols for generated code to use, including the main method for top-level statements.
            if (name.Contains('<') || name.Contains('>'))
            {
                return;
            }

            if (Casing.IsPascalCase(name))
            {
                return;
            }

            ImmutableArray<AttributeData> attributes = namedTypeSymbol.GetAttributes().AddRange(namedTypeSymbol.ContainingType.GetAttributes());
            if (attributes.Any(attribute => attribute.AttributeClass?.Name == nameof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
            {
                return;
            }

            // Skip test methods - they commonly use underscores for readability (e.g., "Method_Scenario_ExpectedResult")
            if (IsTestMethod(namedTypeSymbol))
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_Rule, namedTypeSymbol.Locations[0], name);

            context.ReportDiagnostic(diagnostic);
        }

        private static bool IsTestMethod(IMethodSymbol methodSymbol)
        {
            // Test framework namespaces - checking namespace is more flexible than specific attribute names
            string[] testFrameworkNamespaces = 
            [
                "Xunit",                                    // xUnit (note: namespace is "Xunit", not "XUnit")
                "NUnit.Framework",                          // NUnit
                "Microsoft.VisualStudio.TestTools.UnitTesting",  // MSTest
                "TUnit.Core"                                // TUnit
            ];

            // Fallback attribute names for test environments where namespace metadata may be incomplete
            string[] commonTestAttributeNames =
            [
                "TestMethod", "TestMethodAttribute",        // MSTest
                "Fact", "FactAttribute",                    // xUnit
                "Theory", "TheoryAttribute",                // xUnit
                "Test", "TestAttribute",                    // NUnit
                "TestCase", "TestCaseAttribute",            // NUnit
                "TestCaseSource", "TestCaseSourceAttribute" // NUnit
            ];

            ImmutableArray<AttributeData> attributes = methodSymbol.GetAttributes();
            return attributes.Any(attribute =>
            {
                if (attribute.AttributeClass == null)
                {
                    return false;
                }

                // Check namespace first (more robust for production code)
                string containingNamespace = attribute.AttributeClass.ContainingNamespace?.ToDisplayString();
                if (containingNamespace != null && 
                    testFrameworkNamespaces.Any(ns => containingNamespace.StartsWith(ns, StringComparison.Ordinal)))
                {
                    return true;
                }

                // Fallback: check attribute name for common test attributes
                string attributeName = attribute.AttributeClass.Name;
                return commonTestAttributeNames.Contains(attributeName);
            });
        }
    }
}
