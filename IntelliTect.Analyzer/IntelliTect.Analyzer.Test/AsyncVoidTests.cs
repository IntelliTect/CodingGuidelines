using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class AsyncVoidTests : CodeFixVerifier
    {
        [TestMethod]
        public void AsyncTaskMethod_NoDiagnosticInformationReturned()
        {
            string test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public async Task Sample() { }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AsyncVoidMethod_Warning()
        {
            string test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public async void Sample() { }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0201",
                Message = "Async methods should not return void",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", 13, 31)
                        ]
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public async Task AsyncVoidMethod_CodeFix_ChangesReturnTypeToTask()
        {
            string test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public async void Sample() { }
        }
    }";

            string fixTest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public async Task Sample() { }
        }
    }";
            await VerifyCSharpFix(test, fixTest, allowNewCompilerDiagnostics: true);
        }

        [TestMethod]
        [Description("Analyzer should not crash when encountering non-async non-void methods")]
        public void NonAsyncNonVoidMethod_NoDiagnosticAndNoCrash()
        {
            // The analyzer uses 'as IMethodSymbol' then dereferences without null check.
            // This test ensures it handles the method symbol safely.
            string test = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public void SyncMethod() { }
            public int GetValue() => 42;
            public static void StaticMethod() { }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.AsyncVoid();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.AsyncVoid();
        }
    }
}
