using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class UnusedLocalVariableTests : CodeFixVerifier
    {
        [TestMethod]
        public void InstanceMemberAccessedOnLocalVariable_NoDiagnosticInformationReturned()
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
        public void Foo()
        {
            object foo = new object();
            foo.ToString();
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void LocalVariablePassedToMethod_NoDiagnosticInformationReturned()
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
        public void Foo()
        {
            object foo = new object();
            Bar(foo);
        }

        public void Bar(object bar) { }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void UnusedLocalVariable_NoDiagnosticInformationReturned()
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
        public void Foo()
        {
            object foo = new object();
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = Analyzers.UnusedLocalVariable.DiagnosticId,
                Message = "Local variables should be used",
                Severity = DiagnosticSeverity.Info,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 15, 20)
                }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public async Task UnusedLocalVariable_CodeFix_RemovedUnusedLocalVariable_RemovesVariable()
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
            public void Foo()
            {
                var foo = new object();
            }
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
            public void Foo()
            {
            }
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.UnusedLocalVariable();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.UnusedLocalVariable();
        }
    }
}
