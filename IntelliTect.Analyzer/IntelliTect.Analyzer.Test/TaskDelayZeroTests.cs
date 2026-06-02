using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class TaskDelayZeroTests : CodeFixVerifier
    {
        private const string DiagnosticId = Analyzers.TaskDelayZero.DiagnosticId;
        private const string DiagnosticMessage = "Replace Task.Delay(0) with Task.CompletedTask";
        [TestMethod]
        public void TaskDelayZero_ProducesInfoMessage()
        {
            string source = """
                using System.Threading.Tasks;

                class C
                {
                    void M()
                    {
                        Task.Delay(0);
                    }
                }
                """;

            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Severity = DiagnosticSeverity.Info,
                    Message = DiagnosticMessage,
                    Locations =
                        [
                            new DiagnosticResultLocation("Test0.cs", 7, 9)
                        ]
                });
        }

        [TestMethod]
        public void TaskDelayZeroWithToken_ProducesInfoMessage()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    void M(CancellationToken token)
                    {
                        Task.Delay(0, token);
                    }
                }
                """;

            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Severity = DiagnosticSeverity.Info,
                    Message = DiagnosticMessage,
                    Locations =
                        [
                            new DiagnosticResultLocation("Test0.cs", 8, 9)
                        ]
                });
        }

        [TestMethod]
        public void TaskDelayNonZero_ProducesNoDiagnostic()
        {
            string source = """
                using System.Threading.Tasks;

                class C
                {
                    void M()
                    {
                        Task.Delay(1);
                    }
                }
                """;

            VerifyCSharpDiagnostic(source);
        }

        [TestMethod]
        public async Task TaskDelayZero_CodeFix_UsesCompletedTask()
        {
            string source = """
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0);
                    }
                }
                """;

            string fixedSource = """
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.CompletedTask;
                    }
                }
                """;

            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithToken_CodeFix_PreservesCancellationBehavior()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M(CancellationToken token)
                    {
                        return Task.Delay(0, token);
                    }
                }
                """;

            string fixedSource = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M(CancellationToken token)
                    {
                        return token.IsCancellationRequested ? Task.FromCanceled(token) : Task.CompletedTask;
                    }
                }
                """;

            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithDefaultToken_CodeFix_UsesTypedDefault()
        {
            string source = """
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0, default);
                    }
                }
                """;

            string fixedSource = """
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return default(global::System.Threading.CancellationToken).IsCancellationRequested ? Task.FromCanceled(default) : Task.CompletedTask;
                    }
                }
                """;

            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithSideEffectingTokenExpression_NoCodeFix()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0, GetToken());
                    }

                    CancellationToken GetToken() => default;
                }
                """;

            await VerifyCSharpFix(source, source);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithPropertyTokenExpression_NoCodeFix()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0, Token);
                    }

                    CancellationToken Token => default;
                }
                """;

            await VerifyCSharpFix(source, source);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithCancellationTokenNone_CodeFix_PreservesCancellationBehavior()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0, CancellationToken.None);
                    }
                }
                """;

            string fixedSource = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return CancellationToken.None.IsCancellationRequested ? Task.FromCanceled(CancellationToken.None) : Task.CompletedTask;
                    }
                }
                """;

            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithTimeProvider_NoCodeFix()
        {
            string source = """
                using System;
                using System.Threading.Tasks;

                class C
                {
                    Task M()
                    {
                        return Task.Delay(0, TimeProvider.System);
                    }
                }
                """;

            await VerifyCSharpFix(source, source);
        }

        [TestMethod]
        public async Task TaskDelayZeroWithTimeProviderAndToken_NoCodeFix()
        {
            string source = """
                using System;
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    Task M(CancellationToken token)
                    {
                        return Task.Delay(0, TimeProvider.System, token);
                    }
                }
                """;

            await VerifyCSharpFix(source, source);
        }

        [TestMethod]
        public async Task AwaitTaskDelayZero_CodeFix_UsesCompletedTask()
        {
            string source = """
                using System.Threading.Tasks;

                class C
                {
                    async Task M()
                    {
                        await Task.Delay(0);
                    }
                }
                """;

            string fixedSource = """
                using System.Threading.Tasks;

                class C
                {
                    async Task M()
                    {
                        await Task.CompletedTask;
                    }
                }
                """;

            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public void TaskDelayZeroNamedArguments_ProducesInfoMessage()
        {
            string source = """
                using System.Threading;
                using System.Threading.Tasks;

                class C
                {
                    void M(CancellationToken token)
                    {
                        Task.Delay(cancellationToken: token, millisecondsDelay: 0);
                    }
                }
                """;

            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = DiagnosticId,
                    Severity = DiagnosticSeverity.Info,
                    Message = DiagnosticMessage,
                    Locations =
                        [
                            new DiagnosticResultLocation("Test0.cs", 8, 9)
                        ]
                });
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.TaskDelayZero();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.TaskDelayZero();
        }
    }
}
