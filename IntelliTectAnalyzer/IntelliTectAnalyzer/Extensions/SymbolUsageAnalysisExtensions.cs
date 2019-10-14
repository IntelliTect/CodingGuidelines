/*
The MIT License (MIT)

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
#pragma warning disable RS1012
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1062 // Validate arguments of public methods

using System;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IntelliTectAnalyzer.Extensions
{

    // This is taken from the work done at https://github.com/dotnet/platform-compat

    public struct SymbolUsageAnalysisContext
    {
        public SymbolUsageAnalysisContext(SyntaxNodeAnalysisContext originalContext, ISymbol symbol)
        {
            OriginalContext = originalContext;
            Symbol = symbol;
        }

        private SyntaxNodeAnalysisContext OriginalContext { get; }

        public CancellationToken CancellationToken => OriginalContext.CancellationToken;

        public ISymbol Symbol { get ; }

        public Location GetLocation()
        {
            return OriginalContext.Node.GetLocation();
        }

        public void ReportDiagnostic(Diagnostic diagnostic)
        {
            OriginalContext.ReportDiagnostic(diagnostic);
        }
    }

    public static class SymbolUsageAnalysisExtensions
    {
        public static void RegisterSymbolUsageAction(this AnalysisContext context,
            Action<SymbolUsageAnalysisContext> action)
        {
            RegisterSyntaxNodeAction(context.RegisterSyntaxNodeAction, action);
        }

        public static void RegisterSymbolUsageAction(this CompilationStartAnalysisContext context,
            Action<SymbolUsageAnalysisContext> action)
        {
            RegisterSyntaxNodeAction(context.RegisterSyntaxNodeAction, action);
        }

        private static void RegisterSyntaxNodeAction(
            Action<Action<SyntaxNodeAnalysisContext>, SyntaxKind[]> registrationAction,
            Action<SymbolUsageAnalysisContext> action)
        {
            registrationAction(
                nodeContext => Handle(nodeContext, action),
                new[]
                {
                    SyntaxKind.IdentifierName, SyntaxKind.ObjectCreationExpression,

                    // These are the list of operators that can result in
                    // custom operators:

                    SyntaxKind.AddExpression, SyntaxKind.SubtractExpression, SyntaxKind.MultiplyExpression,
                    SyntaxKind.DivideExpression, SyntaxKind.ModuloExpression, SyntaxKind.LeftShiftExpression,
                    SyntaxKind.RightShiftExpression, SyntaxKind.LogicalOrExpression, SyntaxKind.LogicalAndExpression,
                    SyntaxKind.BitwiseOrExpression, SyntaxKind.BitwiseAndExpression, SyntaxKind.ExclusiveOrExpression,
                    SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression, SyntaxKind.LessThanExpression,
                    SyntaxKind.LessThanOrEqualExpression, SyntaxKind.GreaterThanExpression,
                    SyntaxKind.GreaterThanOrEqualExpression, SyntaxKind.AddAssignmentExpression,
                    SyntaxKind.SubtractAssignmentExpression, SyntaxKind.MultiplyAssignmentExpression,
                    SyntaxKind.DivideAssignmentExpression, SyntaxKind.ModuloAssignmentExpression,
                    SyntaxKind.AndAssignmentExpression, SyntaxKind.ExclusiveOrAssignmentExpression,
                    SyntaxKind.OrAssignmentExpression, SyntaxKind.LeftShiftAssignmentExpression,
                    SyntaxKind.RightShiftAssignmentExpression, SyntaxKind.UnaryPlusExpression,
                    SyntaxKind.UnaryMinusExpression, SyntaxKind.BitwiseNotExpression, SyntaxKind.LogicalNotExpression,
                    SyntaxKind.PreIncrementExpression, SyntaxKind.PreDecrementExpression,
                    SyntaxKind.PostIncrementExpression, SyntaxKind.PostDecrementExpression
                }
            );
        }

        private static void Handle(SyntaxNodeAnalysisContext context, Action<SymbolUsageAnalysisContext> action)
        {
            SyntaxKind syntaxKind = context.Node.Kind();
            switch (syntaxKind)
            {
                case SyntaxKind.IdentifierName:
                case SyntaxKind.ObjectCreationExpression:
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                case SyntaxKind.LogicalOrExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.ExclusiveOrExpression:
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.AddAssignmentExpression:
                case SyntaxKind.SubtractAssignmentExpression:
                case SyntaxKind.MultiplyAssignmentExpression:
                case SyntaxKind.DivideAssignmentExpression:
                case SyntaxKind.ModuloAssignmentExpression:
                case SyntaxKind.AndAssignmentExpression:
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                case SyntaxKind.OrAssignmentExpression:
                case SyntaxKind.LeftShiftAssignmentExpression:
                case SyntaxKind.RightShiftAssignmentExpression:
                case SyntaxKind.UnaryPlusExpression:
                case SyntaxKind.UnaryMinusExpression:
                case SyntaxKind.BitwiseNotExpression:
                case SyntaxKind.LogicalNotExpression:
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                case SyntaxKind.PostIncrementExpression:
                case SyntaxKind.PostDecrementExpression:
                    Handle(context, (ExpressionSyntax)context.Node, action);
                    break;
                default:
                    throw new NotImplementedException($"Unexpected node. Kind = {context.Node.Kind()}");
            }
        }

        private static void Handle(SyntaxNodeAnalysisContext context, ExpressionSyntax node,
            Action<SymbolUsageAnalysisContext> action)
        {
            var symbolInfo = context.SemanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;

            // No point in checking unresolved symbols.
            if (symbol == null)
                return;

            // We don't want to check symbols that aren't best matches.
            if (symbolInfo.CandidateReason != CandidateReason.None)
                return;

            // We don't want to handle generic instantiations, we only
            // care about the original definitions.
            symbol = symbol.OriginalDefinition;

            // We don't want handle synthetic extensions, we only care
            // about the original static declaration.
            if (symbol.Kind == SymbolKind.Method && symbol is IMethodSymbol methodSymbol &&
                methodSymbol.ReducedFrom != null)
            {
                symbol = methodSymbol.ReducedFrom;
            }
            else if (symbol.Kind == SymbolKind.Property && symbol is IPropertySymbol propertySymbol)
            {
                // For properties figure out if it is the getter or setter and then use the corresponding method
                // Many different expressions can have the property as a getter, so the code below attempts to identify all setters.
                var isSetter = false;
                SyntaxNode parent = node.Parent;
                if (parent is MemberAccessExpressionSyntax)
                {
                    isSetter = parent.Parent is AssignmentExpressionSyntax assignmentExpression &&
                               assignmentExpression.Left == parent;
                }
                else if ((parent is AssignmentExpressionSyntax assigmentExpression &&
                          assigmentExpression.Left == node) || parent is NameEqualsSyntax)
                {
                    isSetter = true;
                }

                // It is possible that neither the setter nor the getter actually exists but the null check after this block takes care of that.
                // For the cases that we are interested the method must exist, direct operations on backing fields are not interesting to our analysis.
                symbol = isSetter ? propertySymbol.SetMethod : propertySymbol.GetMethod;
                if (symbol == null)
                    return;
            }

            // We don't want to check symbols defined in source.
            if (symbol.DeclaringSyntaxReferences.Any())
                return;

            var symbolUsageContext = new SymbolUsageAnalysisContext(context, symbol);
            action(symbolUsageContext);
        }
    }
}
