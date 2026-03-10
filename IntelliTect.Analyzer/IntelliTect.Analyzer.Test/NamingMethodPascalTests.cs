using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
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
                Message = "Method 'localMethod' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", 17, 24)
                        ]
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
        public void ProperlyNamedMethod_TopLevelStatements_NoDiagnosticInformationReturned()
        {
            string test = @"
Console.WriteLine(""Hello World!"");";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void AutoProperty_PascalCasedMethod_NoDiagnosticInformationReturned()
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
            public int Number { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 70")]
        public void MethodNotPascalCase_InNativeMethodsClass_NoDiagnosticInformationReturned()
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
        class NativeMethods
        {   
            public string mymethod() 
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
                Message = "Method 'myMethod' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        ]
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
                Message = "Method '_MyMethod' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        ]
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

        [TestMethod]
        [Description("Issue 93")]
        public void MethodWithNamingViolation_InGeneratedCode_Ignored()
        {
            string test = @"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ValidationScriptsPartial), @""mvc.1.0.view"", @""/Views/Shared/_ValidationScriptsPartial.cshtml"")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@""SHA1"", @""9eb468fb4b25bc6ad91d6b1bb06c95bd39c3b25c"", @""/Views/Shared/_ValidationScriptsPartial.cshtml"")]
    public class Views_Shared__ValidationScriptsPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        public void foo() { }
#pragma warning restore 1591
";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 80")]
        public void MethodWithNamingViolation_MethodWithUnderscore_Warning()
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
            public string My_Method() 
            {
                return string.Empty;
            } 
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Method 'My_Method' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        ]
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        [Description("Issue 100")]
        public void ExplicitlyImplementedMethod_PascalCasedMethod_NoDiagnosticInformationReturned()
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
        public interface IInterface
        {
            public void Foo();
        }

        public class TypeName : IInterface
        {
            void IInterface.Foo() { }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 100")]
        public void ExplicitlyImplementedMethod_MethodNotPascalCase_Warnings()
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
        public interface IInterface
        {
            public void foo();
        }

        public class TypeName : IInterface
        {
            void IInterface.foo() { }
        }
    }";

            var expected1 = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Method 'foo' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                        new DiagnosticResultLocation("Test0.cs", 13, 25)
                    ]
            };
            var expected2 = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Method 'foo' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                        new DiagnosticResultLocation("Test0.cs", 18, 29)
                    ]
            };
            VerifyCSharpDiagnostic(test, expected1, expected2);
        }

        public static IEnumerable<object[]> TestFrameworks =>
            TestFrameworkReferences.All.Select(f => new object[] { f.Name, f.TestAttribute, f.UsingDirective });

        [TestMethod]
        [DynamicData(nameof(TestFrameworks))]
        [Description("Issue 371 - Test method with underscores should not trigger INTL0003 for any supported framework")]
        public void TestMethodWithUnderscores_TestFrameworkAttribute_NoDiagnosticInformationReturned(
            string frameworkName, string testAttribute, string usingDirective)
        {
            _ = frameworkName; // parameter used for test identification in output
            string test = $@"
    using System;
    {usingDirective}

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{
            {testAttribute}
            public void FooThing_IsFooThing_HasFooThing() {{ }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 371 - Non-test method with underscores in a test class should still trigger INTL0003")]
        public void NonTestMethodWithUnderscores_InTestClass_DiagnosticReturned()
        {
            string test = @"
    using System;
    using Xunit;

    namespace ConsoleApplication1
    {
        public class TypeName
        {
            [Fact]
            public void FooThing_IsFooThing_HasFooThing() { }

            public void helper_setup() { }
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Method 'helper_setup' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                        new DiagnosticResultLocation("Test0.cs", 12, 25)
                    ]
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        [Description("Issue 371 - User-defined attribute named 'Test' in a non-framework namespace must not suppress INTL0003")]
        public void MethodWithUnderscores_UserDefinedTestAttribute_DiagnosticReturned()
        {
            string test = @"
    using System;

    namespace MyApp
    {
        public class TestAttribute : System.Attribute { }
    }

    namespace ConsoleApplication1
    {
        public class TypeName
        {
            [MyApp.Test]
            public void FooThing_IsFooThing_HasFooThing() { }
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = "INTL0003",
                Message = "Method 'FooThing_IsFooThing_HasFooThing' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                        new DiagnosticResultLocation("Test0.cs", 14, 25)
                    ]
            };

            VerifyCSharpDiagnostic(test, expected);
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
