using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class UnusedLocalVariableTests : CodeFixVerifier
    {
        [TestMethod]
        public void InstanceMemberAccessedOnLocalVariable_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            object foo = new object();
            foo.ToString();
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void LocalVariablePassedToMethod_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            object foo = new object();
            Bar(foo);
        }

        public void Bar(object bar) { }
    }
}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        [Description("77")]
        public void ExpressionBodiedMembers_NoDiagnosticInformationReturned()
        {
            string test = @"
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SecretSanta.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void UnusedLocalVariable_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            object foo = new object();
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = Analyzers.UnusedLocalVariable.DiagnosticId,
                Message = "Local variable 'foo' should be used", 
                Severity = DiagnosticSeverity.Info,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 15, 20)
                }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Descriptor_ContainsExpectedValues()
        {
            DiagnosticAnalyzer analyzer = GetCSharpDiagnosticAnalyzer();
            DiagnosticDescriptor diagnostic = analyzer.SupportedDiagnostics.Single();

            Assert.AreEqual("INTL0303", diagnostic.Id);
            Assert.AreEqual("Local variable unused", diagnostic.Title);
            Assert.AreEqual("Local variable '{0}' should be used", diagnostic.MessageFormat);
            Assert.AreEqual("Flow", diagnostic.Category);
            Assert.AreEqual(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
            Assert.AreEqual(true, diagnostic.IsEnabledByDefault);
            Assert.AreEqual("All local variables should be accessed, or named with underscores to indicate they are unused", diagnostic.Description);
            Assert.AreEqual("https://github.com/IntelliTect/CodingGuidelines", diagnostic.HelpLinkUri);
        }

        [TestMethod]
        public void LambdaExpressionWithDiscard_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            Bar(_ => { return true; });
            bool Bar(Func<bool, bool> func)
            {
                return func(true);
            }
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void LambdaExpressionWithTwoDiscards_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            Bar(__ => { return true; });
            bool Bar(Func<bool, bool> func)
            {
                return func(true);
            }
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void LambdaMethodWithDiscard_NoDiagnosticInformationReturned()
        {
            string test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            Bar(_ => true);
            bool Bar(Func<bool, bool> func)
            {
                return func(true);
            }
        }
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void LambdaMethodWithNamedVar_ReturnsDiagnosticInformation()
        {
            string test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {   
        public void Foo()
        {
            Bar(t => true);
            bool Bar(Func<bool, bool> func)
            {
                return func(true);
            }
        }
    }
}";
            var result = new DiagnosticResult
            {
                Id = "INTL0303",
                Message = "Local variable 't' should be used",
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 17)
                        }
            };
            VerifyCSharpDiagnostic(test, result);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.UnusedLocalVariable();
        }
    }
}
