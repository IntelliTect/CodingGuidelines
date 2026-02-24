using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class AttributesOnSeparateLinesTests : CodeFixVerifier
    {
        [TestMethod]
        public void ProperlySetAttributes_TwoAttributesOnMethod_NoDiagnosticInformationReturned()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    class Program
    {
        [A]
        [B]
        static void Main()
        {
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ProperlySetAttributes_SingleAttributeWithMultipleProperties_NoDiagnosticInformationReturned()
        {
            string test = @"using System;
namespace ConsoleApp
{
    class AAttribute : Attribute
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }

    [A(Foo = ""Foo"", Bar = ""Bar"")]
    class Program
    {
        [A(Foo = ""Foo"", Bar = ""Bar"")]
        static void Main()
        {
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassAttribute_SymbolDeclaredOnSameLineAsAttributeInPartial_NoDiagnosticInformationReturned()
        {
            string test = @"using System;
namespace ConsoleApp
{
    [A(Foo = ""Foo"", Bar = ""Bar"")]
    class Program
    {
        [A(Foo = ""Foo"", Bar = ""Bar"")]
        static void Main()
        {
        }
    }

    class AAttribute : Attribute
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}";

            string test2 = @"using System;
namespace ConsoleApp
{
    partial class Program
    {
    }
}";
            //Testing both here because the order the files are loaded changes
            //the order that location information from the Program symbol is returned.
            VerifyCSharpDiagnostic(new[] { test2, test });
            VerifyCSharpDiagnostic(new[] { test, test2 });
        }

        [TestMethod]
        public void ClassAttribute_DeclaredOnPartialSymbolInTheSameFile_Warning()
        {
            string test = @"using System;
namespace ConsoleApp
{
    class Program
    {
        [A(Foo = ""Foo"", Bar = ""Bar"")]
        static void Main()
        {
        }
    }

    [A] partial class Program
    {
    }
    class AAttribute : Attribute
    {
    }
}";

            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(12, 6));
        }

        [TestMethod]
        public void MethodAttributeLineViolation_TwoAttributesOnSameLine_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    class Program
    {
        [A][B]
        static void Main()
        {
        }
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(15, 13));
        }

        [TestMethod]
        public void ClassAttributeLineViolation_TwoAttributesOnSameLine_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

        [A][B]
    class Program
    {
        static void Main()
        {
        }
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(13, 13));
        }

        [TestMethod]
        public void PropertyAttributeLineViolation_TwoAttributesOnSameLine_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    class Program
    {
        static void Main()
        {
        }

        [A][B]
int Prop {get;set;}
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(19, 13));
        }

        [TestMethod]
        public void MethodAttributeLineViolation_AttributeOnSameLineAsMethodSignature_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class Program
    {
        [A]static void Main()
        {
        }
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(11, 10));
        }

        [TestMethod]
        public void ClassAttributeLineViolation_AttributeOnSameLineAsClassName_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    [A]class Program
    {
        static void Main()
        {
        }
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(9, 6));
        }

        [TestMethod]
        public void PropertyAttributeLineViolation_AttributeOnSameLineAsProperty_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class Program
    {
        static void Main()
        {
        }

        [A]int Prop {get;set;}
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(15, 10));
        }

        [TestMethod]
        public void PropertyAttributeLineViolation_AttributeOnSameLineAsEnum_Warning()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    public enum Foo
    {
        [A]Bar
    }

    class Program
    {
        static void Main()
        {
        }
    }
}";
            VerifyCSharpDiagnostic(test, GetExpectedDiagnosticResult(11, 10));
        }

        [TestMethod]
        public async Task ClassAttributeLineViolation_CodeFix_TwoAttributesOnSameLine_TwoArgumentsOnSeparateLines()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    [A][B]
    class Program
    {
        static void Main()
        {
        }
    }
}";

            string fixTest = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    [A]
    [B]
    class Program
    {
        static void Main()
        {
        }
    }
}";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        public async Task ClassAttributeListLineViolation_CodeFix_TwoAttributesOnSameLine_TwoArgumentsOnSeparateLines()
        {
            string test = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    [A, B]
    class Program
    {
        static void Main()
        {
        }
    }
}";

            string fixTest = @"using System;

namespace ConsoleApp
{
    class AAttribute : Attribute
    {
    }

    class BAttribute : Attribute
    {
    }

    [A]
    [B]
    class Program
    {
        static void Main()
        {
        }
    }
}";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        [Description("Analyzer should not report on generated code")]
        public void AttributesOnSameLine_InGeneratedCode_NoDiagnostic()
        {
            // AttributesOnSeparateLines uses GeneratedCodeAnalysisFlags.Analyze | ReportDiagnostics,
            // meaning it reports inside generated code. It should skip generated code.
            string test = @"using System;
using System.CodeDom.Compiler;

namespace ConsoleApp
{
    class AAttribute : Attribute { }
    class BAttribute : Attribute { }

    [GeneratedCode(""tool"", ""1.0"")]
    class Program
    {
        [A][B]
        static void Main()
        {
        }
    }
}";
            // Should NOT produce a diagnostic for attributes on same line inside generated code
            VerifyCSharpDiagnostic(test);
        }

        private static DiagnosticResult GetExpectedDiagnosticResult(int line, int col)
        {
            return new DiagnosticResult
            {
                Id = "INTL0101",
                Message = "Attributes should be on separate lines",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    [
                            new DiagnosticResultLocation("Test0.cs", line, col)
                        ]
            };
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.AttributesOnSeparateLines();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.AttributesOnSeparateLines();
        }
    }
}
