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
        public void ProperlyNamedLocalMethod_PascalCasedMethod_NoDiagnosticInformationReturned()
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
                localMethod();

                void localMethod() {

                }

                return string.Empty;
            } 
        }
    }";

            VerifyCSharpDiagnostic(test);
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
        public async Task PropertyNotPascalCase_CodeFix_FixNamingViolation_PropertyIsNamedCorrectly()
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
            public string myProperty { get; set; }
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
            public string MyProperty { get; set; }
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        [Description("Issue 13")]
        public void CustomIndexers_ShouldNotNeedToFollowingPropertyNamingScheme()
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
            public int this[int index] => 0;
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 40")]
        public void PropertyWithNamingViolation_PropertyHasGeneratedAttribute_Ignored()
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
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""16.0.0.0"")]
            public string myProperty { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 40")]
        public void MethodWithNamingViolation_ClassHasGeneratedAttribute_Ignored()
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
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""16.0.0.0"")]
        class TypeName
        {   
            public string myMethod()
            {
                return string.Empty();

            } 
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            throw new NotImplementedException();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.NamingMethodPascal();
        }
    }
}
