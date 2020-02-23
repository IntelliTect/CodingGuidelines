using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class NamingPropertyPascalTests : CodeFixVerifier
    {
        [TestMethod]
        public void ProperlyNamedProperty_PascalCasedProperty_NoDiagnosticInformationReturned()
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
            public string MyProperty { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 70")]
        public void PropertyWithNamingViolation_InNativeMethodsClass_NoDiagnosticInformationReturned()
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
            public string myproperty { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyWithNamingViolation_PropertyNotPascalCase_Warning()
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
            var expected = new DiagnosticResult
            {
                Id = "INTL0002",
                Message = "Property 'myProperty' should be PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void PropertyWithUnderScoreAsFirstChar_PropertyNotPascalCase_Warning()
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
            public string _MyProperty { get; set; }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0002",
                Message = "Property '_MyProperty' should be PascalCase",
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
        public void PropertyWithNamingViolation_ClassHasGeneratedAttribute_Ignored()
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
            public string myProperty { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 93")]
        public void PropertyWithNamingViolation_InGeneratedCode_Ignored()
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
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
#pragma warning restore 1591
";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.NamingIdentifierPascal();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.NamingPropertyPascal();
        }
    }
}
