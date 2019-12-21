using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class NamingMethodPascalTests : CodeFixVerifier
    {
        [TestMethod]
        public void LocalMethodWithLowerCaseFirstChar_MethodNotPascalCase_Warning()
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
            public string MyMethod() 
            {
                var output = localMethod();

                string localMethod() {
                    return string.Empty;
                }

                return output;
            } 
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Methods should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 24)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void ProperlyNamedMethod_PascalCasedMethod_NoDiagnosticInformationReturned()
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
            public string MyMethod() 
            {
                return string.Empty;
            } 
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodWithNamingViolation_MethodNotPascalCase_Warning()
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
            public string myMethod() 
            {
                return string.Empty;
            } 
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Methods should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void MethodWithUnderScoreAsFirstChar_MethodNotPascalCase_Warning()
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
            public string _MyMethod() 
            {
                return string.Empty;
            } 
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Methods should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public async Task MethodNotPascalCase_CodeFix_FixNamingViolation_MethodIsNamedCorrectly()
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
            public string myMethod() 
            {
                return string.Empty;
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
            public string MyMethod() 
            {
                return string.Empty;
            }
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        public async Task MethodNotPascalCase_CodeFix_FixNamingViolation_LocalFunctionIsNamedCorrectly()
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
            public string getEmpty() {

                var output = localFunction();

                string localFunction() {
                    return string.Empty();
                }

                return output;
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
            public string GetEmpty() {

                var output = LocalFunction();

                string LocalFunction() {
                    return string.Empty();
                }

                return output;
            }
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }


        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.NamingIdentifierPascal();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.NamingMethodPascal();
        }
    }
}
