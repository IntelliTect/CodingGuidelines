using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class NamingPropertyPascalTests : CodeFixVerifier
    {
        [TestMethod]
        public void ProperlyNamedProperty_PascalCasedProperty_NoDiagnosticInformationReturned()
        {
            var test = @"
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
        public void PropertyWithNamingViolation_PropertyNotPascalCase_Warning()
        {
            var test = @"
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
                Message = "Properties should be PascalCase",
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
            var test = @"
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

            var fixTest = @"
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
            var test = @"
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

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.NamingPropertyPascal();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.NamingPropertyPascal();
        }
    }
}
