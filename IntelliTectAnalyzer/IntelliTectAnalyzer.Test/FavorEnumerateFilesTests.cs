using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class FavorEnumerateFilesTests : CodeFixVerifier
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
                    Id = "INTL0100",
                    Severity = DiagnosticSeverity.Info
                });
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.FavorEnumerateFiles();
        }
    }
}
