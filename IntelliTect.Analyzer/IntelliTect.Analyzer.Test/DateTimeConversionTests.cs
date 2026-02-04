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
        public void UsageOfDateTimeInWhereLambda_DoesNotTriggerWarning_KnownLimitation()
        {
            // NOTE: This test documents a known limitation of the Roslyn operation-based analyzer.
            // INTL0202 currently does NOT trigger for implicit conversions in LINQ extension method lambdas
            // (e.g., System.Linq.Enumerable.Where) due to how Roslyn handles conversion operations
            // in these contexts.
            //
            // Related: https://github.com/dotnet/roslyn/issues/14722
            //
            // This limitation affects:
            // - Extension methods from System.Linq.Enumerable (Where, Select, etc.)
            // - Potentially other extension method lambdas
            //
            // The analyzer DOES work correctly for:
            // - Direct comparisons (e.g., if (dateTime < dateTimeOffset))
            // - Local function calls (see UsageOfDateTimeInLocalFunction_ProducesWarningMessage)
            // - Instance method lambdas (e.g., List<T>.RemoveAll - see UsageOfDateTimeInRemoveAllLambda_ProducesWarningMessage)
            //
            // TODO: Investigate syntax-based analysis or other workarounds to detect these cases
            
            string source = @"
using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp48
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<DateTimeOffset>();
            DateTime dt = DateTime.Now;
            
            _ = list.Where(item => item < dt);
        }
    }
}";

            // Currently no warnings - this documents the known limitation
            // Explicitly expecting zero diagnostics
            VerifyCSharpDiagnostic(source);
        }

        [TestMethod]
        public void UsageOfDateTimeInRemoveAllLambda_ProducesWarningMessage()
        {
            // This test shows that RemoveAll lambda DOES trigger warning
            string source = @"
using System;
using System.Collections.Generic;

namespace ConsoleApp48a
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<DateTimeOffset>();
            DateTime dt = DateTime.Now;
            
            list.RemoveAll(item => item < dt);
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
                            new DiagnosticResultLocation("Test0.cs", 14, 43)
                                   ]
                           });

        }

        [TestMethod]
        public void UsageOfDateTimeInLocalFunction_ProducesWarningMessage()
        {
            string source = @"
using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp49
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<DateTimeOffset>();
            DateTime dt = DateTime.Now;
            
            bool Compare(DateTimeOffset item)
            {
                return item < dt;
            }
            
            var query = list.Where(Compare);
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
                            new DiagnosticResultLocation("Test0.cs", 17, 31)
                                   ]
                           });

        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.BanImplicitDateTimeToDateTimeOffsetConversion();
        }
    }
}
