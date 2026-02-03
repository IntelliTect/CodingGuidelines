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
                                   [
                            new DiagnosticResultLocation("Test0.cs", 10, 38)
                                   ]
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
            _ = first < second;
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
                                   [new DiagnosticResultLocation("Test0.cs", 13, 17)]
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
            _ = first < second;
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
                                   [new DiagnosticResultLocation("Test0.cs", 13, 25)]
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
            _ = first < second;
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

        [Ignore("Property-based conversion detection not yet implemented - requires Roslyn IOperation tree investigation")]
        [TestMethod]
        public void UsageOfImplicitConversion_WithProperties_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Pair(DateTimeOffset DateTimeOffset, DateTime DateTime);

    internal class Program
    {
        static void Main(string[] args)
        {
            Pair pair = new(DateTimeOffset.Now, DateTime.Now);
            _ = pair.DateTimeOffset < pair.DateTime;
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
                    Locations = [new DiagnosticResultLocation("Test0.cs", 14, 29)]
                }
            );
        }

        [Ignore("Property-based conversion detection not yet implemented - requires Roslyn IOperation tree investigation")]
        [TestMethod]
        public void UsageOfImplicitConversion_WithPropertiesInLinq_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Pair(DateTimeOffset DateTimeOffset, DateTime DateTime);

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pair> list = new(){ new(DateTimeOffset.Now, DateTime.Now) };
            _ = list.Where(pair => pair.DateTimeOffset < pair.DateTime);
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
                    Locations = [new DiagnosticResultLocation("Test0.cs", 14, 42)]
                }
            );
        }

        [Ignore("Property-based conversion detection not yet implemented - requires Roslyn IOperation tree investigation")]
        [TestMethod]
        public void UsageOfImplicitConversion_InLinqWithVariables_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Pair(DateTimeOffset DateTimeOffset, DateTime DateTime);

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pair> list = new(){ new(DateTimeOffset.Now, DateTime.Now) };
            _ = list.Where(pair => {
                DateTimeOffset first = pair.DateTimeOffset;
                DateTime second = pair.DateTime;
                return first < second;
            });
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
                    Locations = [new DiagnosticResultLocation("Test0.cs", 18, 24)]
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

        [Ignore("Property-based conversion detection not yet implemented - requires Roslyn IOperation tree investigation")]
        [TestMethod]
        public void UsageInLambdaWithDateProperty_ProducesWarningMessage()
        {
            // This test matches the original issue scenario
            string source = @"
using System;
using System.Linq;

namespace Test
{
    public class TimeEntry
    {
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
    }

    public class DataSource
    {
        public IQueryable<TimeEntry> GetQuery(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Empty<TimeEntry>().AsQueryable()
                .Where(te =>
                    te.EndDate <= endDate.Date.AddDays(1).AddTicks(-1) &&
                    te.StartDate >= startDate.Date);
        }
    }
}";

            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = "INTL0202",
                    Severity = DiagnosticSeverity.Warning,
                    Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                    Locations = [new DiagnosticResultLocation("Test0.cs", 18, 35)]
                },
                new DiagnosticResult
                {
                    Id = "INTL0202",
                    Severity = DiagnosticSeverity.Warning,
                    Message = "Using the symbol 'DateTimeOffset.implicit operator DateTimeOffset(DateTime)' can result in unpredictable behavior",
                    Locations = [new DiagnosticResultLocation("Test0.cs", 19, 38)]
                });
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.BanImplicitDateTimeToDateTimeOffsetConversion();
        }
    }
}
