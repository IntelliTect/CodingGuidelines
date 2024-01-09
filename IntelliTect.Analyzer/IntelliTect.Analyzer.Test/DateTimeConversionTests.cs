using System;
using System.IO;
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
                               Locations = [new DiagnosticResultLocation("Test0.cs", 10, 38)]
                           });

        }

        [TestMethod]
        public void UsageOfImplicitConversionInComparison_DateTimeToDateTimeOffset_ProducesWarningMessage()
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
            DateTimeOffset second = DateTimeOffset.Now;
            _ = first < second
        }
    }
}";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations = [new DiagnosticResultLocation("Test0.cs", 17, 17)]
                           });

        }

        [TestMethod]
        public void UsageOfImplicitConversionInComparison_DateTimeOffsetToDateTime_ProducesWarningMessage()
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
            DateTimeOffset first = DateTimeOffset.Now;
            DateTime second = DateTime.Now;
            _ = first < second
        }
    }
}";

            VerifyCSharpDiagnostic(source,
                           new DiagnosticResult
                           {
                               Id = "INTL0202",
                               Severity = DiagnosticSeverity.Warning,
                               Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                               Locations = [new DiagnosticResultLocation("Test0.cs", 13, 25)]
                           });

        }

        [TestMethod]
        public void UsageOfImplicitConversionInComparison_NullableDateTimeOffsetToDateTime_ProducesWarningMessage()
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
            DateTimeOffset? first = DateTimeOffset.Now;
            DateTime second = DateTime.Now;
            _ = first < second
        }
    }
}";

            VerifyCSharpDiagnostic(
                source,
                new DiagnosticResult
                {
                    Id = "INTL0202",
                    Severity = DiagnosticSeverity.Warning,
                    Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                    Locations = [new DiagnosticResultLocation("Test0.cs", 13, 25)]
                }
            );

        }

        [TestMethod]
        public void UsageOfImplicitConversion_InLinq_ProducesWarningMessage()
        {
            string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string path = Path.Combine(dir, "Data", "DateTime_ImplicitConversion_InLinq.cs");
            string source = File.ReadAllText(path);

            VerifyCSharpDiagnostic(
                source,
                new DiagnosticResult
                {
                    Id = "INTL0202",
                    Severity = DiagnosticSeverity.Warning,
                    Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                    Locations = [new DiagnosticResultLocation("Test0.cs", 17, 25)]
                }
            );
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
