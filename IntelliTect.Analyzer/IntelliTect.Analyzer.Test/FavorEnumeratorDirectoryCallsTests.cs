using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class FavorEnumeratorDirectoryCallsTests : CodeFixVerifier
    {
        [TestMethod]
        public void UsageOfDirectoryGetFiles_ProducesInfoMessage()
        {
            string source = @"using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);

            foreach (string file in files)
            {
                Console.WriteLine($""File found: ${file}"");
            }
        }
    }
}";
            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = "INTL0301",
                    Severity = DiagnosticSeverity.Info,
                    Message = "Favor using the method `EnumerateFiles` over the `GetFiles` method.",
                    Locations =
                        new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 30)
                        }
                });
        }

        [TestMethod]
        public void DeclarationOfOtherDirectoryGetFiles_ProducesNothing()
        {
            string source = @"using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp5
{
    public static class Directory {
        public static string[] GetFiles(string path) => Array.Empty<string>();
    }


    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);

        }
    }
}";
            VerifyCSharpDiagnostic(source);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.FavorDirectoryEnumerationCalls();
        }



        [TestMethod]
        public void UsageOfDirectoryGetDirectories_ProducesInfoMessage()
        {
            string source = @"using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);

            foreach (string file in files)
            {
                Console.WriteLine($""File found: ${file}"");
            }
        }
    }
}";
            VerifyCSharpDiagnostic(source,
                new DiagnosticResult
                {
                    Id = "INTL0302",
                    Severity = DiagnosticSeverity.Info,
                    Message = "Favor using the method `EnumerateDirectories` over the `GetDirectories` method.",
                    Locations =
                        new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 30)
                        }
                });
        }

        [TestMethod]
        public void DeclarationOfOtherDirectoryGetDirectories_ProducesNothing()
        {
            string source = @"using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp5
{
    public static class Directory {
        public static string[] GetDirectories(string path) => Array.Empty<string>();
    }


    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);

        }
    }
}";
            VerifyCSharpDiagnostic(source);
        }

        [TestMethod]
        [Description("Issue 53")]
        public void Diagnostic_HandlesMemberAccess()
        {
            string source = @"
using System;

namespace Namespace
{
    class Program
    {
        static void Main(string[] args)
        {
            int selection;
            if (!int.TryParse(Console.ReadLine(), out selection))
            {
                selection = 0;
            }
        }
    }
}
";
            VerifyCSharpDiagnostic(source);
        }
    }
}
