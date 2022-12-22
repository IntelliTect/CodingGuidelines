using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class DateTimeConversionTests : CodeFixVerifier
    {
        [TestMethod]
        public void UsageOfImplicitConversion_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp42
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset ofs = DateTime.Now;
            }
        }
    }";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 38)
                                   }
                           });

        }

        [TestMethod]
        public void UsageOfImplicitConversionInComparison_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime first = DateTime.Now;

            Thread.Sleep(10);

            DateTimeOffset second = DateTimeOffset.Now;

            if (first < second)
            {
                Console.WriteLine(""Time has passed..."");
            }
        }
    }
}";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 17)
                                   }
                           });

        }

        [TestMethod]
        public void UsageOfExplicitConversion_ProducesNothing()
        {
            string source = @"
using System;

namespace ConsoleApp43
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset ofs = (DateTimeOffset)DateTime.Now;
            }
        }
    }";

            VerifyCSharpDiagnostic(source);

        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.BanImplicitDateTimeToDateTimeOffsetConversion();
        }
    }
}
