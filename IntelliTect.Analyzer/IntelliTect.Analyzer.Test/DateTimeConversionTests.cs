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
                               Message = "Using 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' or 'new DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   [
                            new DiagnosticResultLocation("Test0.cs", 10, 38)
                                   ]
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
                               Message = "Using 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' or 'new DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   [
                            new DiagnosticResultLocation("Test0.cs", 17, 17)
                                   ]
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

        [TestMethod]
        public void UsageOfDateTimeOffsetConstructorWithDateTime_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp44
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset ofs = new DateTimeOffset(DateTime.Now);
            }
        }
    }";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' or 'new DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   [
                            new DiagnosticResultLocation("Test0.cs", 10, 38)
                                   ]
                           });

        }

        [TestMethod]
        public void UsageOfDateTimeOffsetConstructorWithDateTimeAndTimeSpan_ProducesNothing()
        {
            string source = @"
using System;

namespace ConsoleApp45
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset ofs = new DateTimeOffset(DateTime.Now, TimeSpan.Zero);
            }
        }
    }";

            VerifyCSharpDiagnostic(source);

        }

        [TestMethod]
        public void UsageOfDateTimeOffsetConstructorWithYearMonthDay_ProducesNothing()
        {
            string source = @"
using System;

namespace ConsoleApp46
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset ofs = new DateTimeOffset(2022, 2, 2, 0, 0, 0, TimeSpan.Zero);
            }
        }
    }";

            VerifyCSharpDiagnostic(source);

        }

        [TestMethod]
        public void UsageOfDateTimeOffsetConstructorWithNewDateTime_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp47
    {
        class Program
        {
            static void Main(string[] args)
            {
                DateTimeOffset dto = new DateTimeOffset(new DateTime(2022, 2, 2));
            }
        }
    }";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' or 'new DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations =
                                   [
                            new DiagnosticResultLocation("Test0.cs", 10, 38)
                                   ]
                           });

        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.BanImplicitDateTimeToDateTimeOffsetConversion();
        }
    }
}
