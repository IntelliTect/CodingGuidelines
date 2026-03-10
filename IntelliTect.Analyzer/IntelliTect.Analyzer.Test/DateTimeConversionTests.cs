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

        [TestMethod]
        public void ImplicitConversionInPropertyComparisonInsideLambda_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp53
{
    class TimeEntry
    {
        public DateTimeOffset EndDate { get; set; }
    }

    class Program
    {
        static DateTimeOffset EndDate { get; set; }

        static void Main(string[] args)
        {
            TimeEntry entry = new TimeEntry();
            Func<bool> check = () => entry.EndDate <= EndDate.Date.AddDays(1).AddTicks(-1);
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
                            new DiagnosticResultLocation("Test0.cs", 18, 55)
                                   ]
                           });
        }

        [TestMethod]
        public void ImplicitConversionInExpressionTreeLambda_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Linq.Expressions;

namespace ConsoleApp54
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<DateTimeOffset, bool>> expr = dto => dto > DateTime.Now;
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
                            new DiagnosticResultLocation("Test0.cs", 11, 72)
                                   ]
                           });
        }

        [TestMethod]
        public void NullableDateTimeToNullableDateTimeOffsetAssignment_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp55
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime? dt = DateTime.Now;
            DateTimeOffset? dto = dt;
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
                            new DiagnosticResultLocation("Test0.cs", 11, 35)
                                   ]
                           });
        }

        [TestMethod]
        public void ImplicitConversionInMethodReturnStatement_ProducesWarningMessage()
        {
            string source = @"
using System;

namespace ConsoleApp57
{
    class Program
    {
        static DateTimeOffset GetDateTimeOffset()
        {
            return DateTime.Now;
        }

        static void Main(string[] args)
        {
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
                            new DiagnosticResultLocation("Test0.cs", 10, 20)
                                   ]
                           });
        }

        [TestMethod]
        public void NullableDateTimeToDateTimeOffsetComparison_ProducesWarningMessage()
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
            DateTime? first = DateTime.Now;

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
        public void NullableDateTimeToNullableDateTimeOffsetComparison_ProducesWarningMessage()
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
            DateTime? first = DateTime.Now;

            Thread.Sleep(10);

            DateTimeOffset? second = DateTimeOffset.Now;

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
        public void DateTimeOffsetLessThanDateTime_ProducesDiagnostic()
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

            Thread.Sleep(10);

            DateTime second = DateTime.Now;

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
                            new DiagnosticResultLocation("Test0.cs", 17, 25)
                                   ]
                           });
        }

        [TestMethod]
        public void NullableDateTimeOffsetLessThanDateTime_ProducesDiagnostic()
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

            Thread.Sleep(10);

            DateTime second = DateTime.Now;

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
                            new DiagnosticResultLocation("Test0.cs", 17, 25)
                                   ]
                           });
        }

        [Ignore]
        [TestMethod]
        public void PropertyBasedDateTimeLessThanDateTimeOffset_ProducesDiagnostic()
        {
            string source = @"
using System;

namespace ConsoleApp1
{
    class Pair
    {
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Pair pair = new Pair();
            _ = pair.DateTime < pair.DateTimeOffset;
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
                            new DiagnosticResultLocation("Test0.cs", 18, 17)
                                   ]
                           });
        }

        [Ignore]
        [TestMethod]
        public void PropertyComparisonInsideLinqLambda_ProducesDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Pair
    {
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pair> list = new List<Pair>();
            _ = list.Where(pair => pair.DateTime < pair.DateTimeOffset);
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
                            new DiagnosticResultLocation("Test0.cs", 20, 36)
                                   ]
                           });
        }

        [Ignore]
        [TestMethod]
        public void ExtractedPropertyVariablesInLinqLambda_ProducesDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Pair
    {
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pair> list = new List<Pair>();
            _ = list.Where(pair =>
            {
                DateTime dt = pair.DateTime;
                DateTimeOffset dto = pair.DateTimeOffset;
                return dt < dto;
            });
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
                            new DiagnosticResultLocation("Test0.cs", 24, 24)
                                   ]
                           });
        }

        [Ignore]
        [TestMethod]
        public void IQueryableWhereWithDateTimePropertyComparison_ProducesDiagnostic()
        {
            string source = @"
using System;
using System.Linq;

namespace ConsoleApp1
{
    class TimeEntry
    {
        public DateTimeOffset EndDate { get; set; }
    }

    internal class Program
    {
        static DateTimeOffset EndDate { get; set; }

        static void Main(string[] args)
        {
            IQueryable<TimeEntry> entries = null;
            _ = entries.Where(te => te.EndDate <= EndDate.Date.AddDays(1).AddTicks(-1));
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
                            new DiagnosticResultLocation("Test0.cs", 20, 44)
                                   ]
                           });
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.BanImplicitDateTimeToDateTimeOffsetConversion();
        }
    }
}
