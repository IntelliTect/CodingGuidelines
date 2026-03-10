
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

            if (namedTypeSymbol.HasGeneratedCodeAttribute(context.Compilation))
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

        // Test framework namespaces — any method decorated with an attribute from these namespaces
        // is considered a test method and exempt from PascalCase validation.
        // To add a new framework, append its root namespace here and update TestFrameworkReferences.cs.
        private static readonly string[] s_testFrameworkNamespaces =
        [
            "Xunit",                                         // xUnit (namespace is "Xunit", not "XUnit")
            "NUnit.Framework",                               // NUnit
            "Microsoft.VisualStudio.TestTools.UnitTesting",  // MSTest
            "TUnit.Core"                                     // TUnit
        ];

        // Fallback attribute names for compilations without framework assembly references.
        // Only used when the attribute type is unresolved (TypeKind.Error).
        private static readonly string[] s_commonTestAttributeNames =
        [
            "TestMethod", "TestMethodAttribute",         // MSTest
            "Fact", "FactAttribute",                     // xUnit
            "Theory", "TheoryAttribute",                 // xUnit
            "Test", "TestAttribute",                     // NUnit / TUnit
            "TestCase", "TestCaseAttribute",             // NUnit
            "TestCaseSource", "TestCaseSourceAttribute"  // NUnit
        ];

        private static bool IsTestMethod(IMethodSymbol methodSymbol)
        {
            ImmutableArray<AttributeData> attributes = methodSymbol.GetAttributes();
            return attributes.Any(attribute =>
            {
                if (attribute.AttributeClass == null)
                {
                    return false;
                }

                // Check namespace first — works whenever the compilation includes real framework references.
                string containingNamespace = attribute.AttributeClass.ContainingNamespace?.ToDisplayString();
                if (containingNamespace != null &&
                    s_testFrameworkNamespaces.Any(ns => containingNamespace.StartsWith(ns, StringComparison.Ordinal)))
                {
                    return true;
                }

                // Fallback: check by name only for unresolved types (missing assembly reference in compilation).
                // Gated on TypeKind.Error to avoid false negatives for user-defined attributes with the same names.
                if (attribute.AttributeClass.TypeKind == TypeKind.Error)
                {
                    return s_commonTestAttributeNames.Contains(attribute.AttributeClass.Name);
                }

                return false;
            });
        }
    }
}
