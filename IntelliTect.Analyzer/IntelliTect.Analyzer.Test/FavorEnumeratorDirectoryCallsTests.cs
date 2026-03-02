using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
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
                    Message = "Favor using the method `EnumerateFiles` over the `GetFiles` method",
                    Locations =
                        [
                            new DiagnosticResultLocation("Test0.cs", 11, 30)
                        ]
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

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.FavorDirectoryEnumerationCalls();
        }

        [TestMethod]
        public async Task GetFiles_AssignedToStringArray_CodeFix_WrapsWithToArray()
        {
            string source = @"using System;
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
            string fixedSource = @"using System;
using System.IO;
using System.Linq;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory).ToArray();

            foreach (string file in files)
            {
                Console.WriteLine($""File found: ${file}"");
            }
        }
    }
}";
            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task GetFiles_UsedInForeach_CodeFix_SimpleRename()
        {
            string source = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                Console.WriteLine(file);
            }
        }
    }
}";
            string fixedSource = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                Console.WriteLine(file);
            }
        }
    }
}";
            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task GetDirectories_AssignedToStringArray_CodeFix_WrapsWithToArray()
        {
            string source = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] dirs = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);

            foreach (string dir in dirs)
            {
                Console.WriteLine($""Directory found: ${dir}"");
            }
        }
    }
}";
            string fixedSource = @"using System;
using System.IO;
using System.Linq;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] dirs = Directory.EnumerateDirectories(AppDomain.CurrentDomain.BaseDirectory).ToArray();

            foreach (string dir in dirs)
            {
                Console.WriteLine($""Directory found: ${dir}"");
            }
        }
    }
}";
            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task GetDirectories_UsedInForeach_CodeFix_SimpleRename()
        {
            string source = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string dir in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory))
            {
                Console.WriteLine(dir);
            }
        }
    }
}";
            string fixedSource = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string dir in Directory.EnumerateDirectories(AppDomain.CurrentDomain.BaseDirectory))
            {
                Console.WriteLine(dir);
            }
        }
    }
}";
            await VerifyCSharpFix(source, fixedSource);
        }

        [TestMethod]
        public async Task GetFiles_ExpressionBodiedMethod_CodeFix_WrapsWithToArray()
        {
            string source = @"using System;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static string[] GetAllFiles(string path) => Directory.GetFiles(path);
    }
}";
            string fixedSource = @"using System;
using System.IO;
using System.Linq;

namespace ConsoleApp5
{
    class Program
    {
        static string[] GetAllFiles(string path) => Directory.EnumerateFiles(path).ToArray();
    }
}";
            await VerifyCSharpFix(source, fixedSource);
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
                    Message = "Favor using the method `EnumerateDirectories` over the `GetDirectories` method",
                    Locations =
                        [
                            new DiagnosticResultLocation("Test0.cs", 11, 30)
                        ]
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
