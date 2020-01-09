
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
    public class NamingMethodPascal : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0003";
        private const string Title = "Methods PascalCase";
        private const string MessageFormat = "Methods should be PascalCase";
        private const string Description = "All methods should be in the format PascalCase";
        private const string Category = "Naming";
        private static readonly string _HelpLinkUri = DiagnosticUrlBuilder.GetUrl(AnalyzerBlock.Naming, 
            DiagnosticId);

        private static readonly DiagnosticDescriptor _Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, 
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description,_HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.LocalFunctionStatement);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var localFunctionStatement = (LocalFunctionStatementSyntax) context.Node;

            if (char.IsUpper(localFunctionStatement.Identifier.Text.First()))
            {
                return;
            } 

            Diagnostic diagnostic = Diagnostic.Create(_Rule, localFunctionStatement.Identifier.GetLocation(), localFunctionStatement.Identifier.Text);

            context.ReportDiagnostic(diagnostic);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            ISymbol namedTypeSymbol = context.Symbol;

            if (!namedTypeSymbol.CanBeReferencedByName)
            {
                return;
            }

            if (namedTypeSymbol.ContainingType.IsNativeMethodsClass())
            {
                return;
            }

            if (char.IsUpper(namedTypeSymbol.Name.First()))
            {
                return;
            }

            ImmutableArray<AttributeData> attributes = namedTypeSymbol.GetAttributes().AddRange(namedTypeSymbol.ContainingType.GetAttributes());
            if (attributes.Any(attribute => attribute.AttributeClass?.Name == nameof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
