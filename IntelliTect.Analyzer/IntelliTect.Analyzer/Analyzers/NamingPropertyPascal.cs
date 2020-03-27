using System;
using System.Collections.Immutable;
using System.Linq;
using IntelliTect.Analyzer.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTect.Analyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingPropertyPascal : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "INTL0002";
        private const string Title = "Properties PascalCase";
        private const string MessageFormat = "Property '{0}' should be PascalCase";
        private const string Description = "All properties should be in the format PascalCase";
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
            
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (IPropertySymbol)context.Symbol;

            ImmutableArray<AttributeData> attributes = namedTypeSymbol.GetAttributes().AddRange(namedTypeSymbol.ContainingType.GetAttributes());
            if (attributes.Any(attribute => attribute.AttributeClass?.Name == nameof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
            {
                return;
            }

            if (namedTypeSymbol.ContainingType.IsNativeMethodsClass())
            {
                return;
            }

            if (namedTypeSymbol.IsIndexer)
            {
                return;
            }

            string name = namedTypeSymbol.ExplicitInterfaceImplementations.FirstOrDefault()?.Name ??
                          namedTypeSymbol.Name;

            if (Casing.IsPascalCase(name))
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(_Rule, namedTypeSymbol.Locations[0], name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
