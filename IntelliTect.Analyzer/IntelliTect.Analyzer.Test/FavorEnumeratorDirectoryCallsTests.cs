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

        [TestMethod]
        [Description("Cast<IdentifierNameSyntax>() throws InvalidCastException on generic method calls")]
        public void GenericMethodCallOnDirectory_DoesNotThrow()
        {
            // memberAccess.ChildNodes().Cast<IdentifierNameSyntax>() will throw
            // if any child node is not IdentifierNameSyntax (e.g. GenericNameSyntax).
            // This test uses a generic method call on a class named Directory.
            string source = @"
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public static class Directory
    {
        public static List<T> GetItems<T>() => new List<T>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var items = Directory.GetItems<string>();
        }
    }
}";
            VerifyCSharpDiagnostic(source);
        }

        [TestMethod]
        [Description("Analyzer should not report when symbol is unresolved (compile error)")]
        public void UnresolvableDirectoryType_NoDiagnostic()
        {
            // When symbol.Symbol is null it means the code has a compile error,
            // not that it's System.IO.Directory. Should not produce a false positive.
            string source = @"
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(""."");
        }
    }
}";
            // No 'using System.IO' so Directory is unresolvable — should NOT produce diagnostic
            VerifyCSharpDiagnostic(source);
        }

        [TestMethod]
        public void DirectoryIdentifier_CaseInsensitiveOrdinal_ProducesInfoMessage()
        {
            string source = @"
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(""."");
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
        [Description("Detect fully-qualified System.IO.Directory.GetFiles()")]
        public void FullyQualifiedDirectoryGetFiles_ProducesInfoMessage()
        {
            string source = @"
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
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
                            new DiagnosticResultLocation("Test0.cs", 10, 30)
                        ]
                });
        }

        [TestMethod]
        [Description("Detect fully-qualified System.IO.Directory.GetDirectories()")]
        public void FullyQualifiedDirectoryGetDirectories_ProducesInfoMessage()
        {
            string source = @"
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] dirs = System.IO.Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
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
                            new DiagnosticResultLocation("Test0.cs", 10, 29)
                        ]
                });
        }
    }
}
