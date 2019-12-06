using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
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

        private DiagnosticResult GetExpectedDiagnosticResult(int line, int col)
        {
            return new DiagnosticResult
            {
                Id = "INTL0101",
                Message = "Attributes should be on separate lines",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", line, col)
                        }
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
