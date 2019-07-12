﻿using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingPropertyPascal : DiagnosticAnalyzer
    {
        public const string _DiagnosticId = "INTL0002";
        private const string _Title = "Properties PascalCase";
        private const string _MessageFormat = "Properties should be PascalCase";
        private const string _Description = "All properties should be in the format PascalCase";
        private const string _Category = "Naming";
        private const string _HelpLinkUri = "https://github.com/IntelliTect/CodingStandards";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(_DiagnosticId, _Title, _MessageFormat, 
            _Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: _Description,_HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol;
            if (char.IsUpper(namedTypeSymbol.Name.First()))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }
}