using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class NamingFieldPascalUnderScoreTests : CodeFixVerifier
    {
        [TestMethod]
        public void ProperlyNamedField_UnderScorePascalCaseFieldAndProperty_NoDiagnosticInformationReturned()
        {
            var test = @"
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
            public string _MyField;
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void FieldWithNamingViolation_FieldMissingLeadingUnderscore_Warning()
        {
            var test = @"
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
            public string MyField;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0001",
                Message = "Fields should be named _PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void FieldWithNamingViolation_FieldNotInPascalCase_Warning()
        {
            var test = @"
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
            public string _myField;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0001",
                Message = "Fields should be named _PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 27)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }
        
        [TestMethod]
        public void FieldWithNamingViolation_FieldTwoLeadingUnderScores_Warning()
        {
            var test = @"
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
            public string __myField;
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "INTL0001",
                Message = "Fields should be named _PascalCase",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 13, 27)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public async Task FieldMissingLeadingUnderscore_CodeFix_FixNamingViolation_FieldIsNamedCorrectly()
        {
            var test = @"
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
            public string MyField;
        }
    }";

            var fixTest = @"
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
            public string _MyField;
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        public async Task FieldNotInPascalCase_CodeFix_FixNamingViolation_FieldIsNamedCorrectly()
        {
            var test = @"
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
            public string _myField;
        }
    }";

            var fixTest = @"
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
            public string _MyField;
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }
        
        [TestMethod]
        public async Task FieldTwoLeadingUnderScores_CodeFix_FixNamingViolation_FieldIsNamedCorrectly()
        {
            var test = @"
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
            public string __myField;
        }
    }";

            var fixTest = @"
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
            public string _MyField;
        }
    }";
            await VerifyCSharpFix(test, fixTest);
        }

        [TestMethod]
        [Description("Issue 10")]
        public void EnumMembers_ShouldNotBeFlagged()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public enum Foo
        {
            None,
            One,
            Two
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [Description("Issue 11")]
        public void ConstantField_DoesNotPromptWarning()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class Program
        {
            public const int Foo = 42;
        }
    }";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        [Description("Issue 15")]
        public void GeneratedCodeInCodeBehind_DoesNotPromptWarning()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace WpfApplication
    {
        public class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1823:AvoidUnusedPrivateFields"")]
            internal System.Windows.Controls.Grid Foo;

            private bool _contentLoaded;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.CodeDom.Compiler.GeneratedCodeAttribute(""PresentationBuildTasks"", ""4.0.0.0"")]
            public void InitializeComponent() {
                if (_contentLoaded) {
                    return;
                }
                _contentLoaded = true;
                System.Uri resourceLocater = new System.Uri(""/WpfApplication;component/mainwindow.xaml"", System.UriKind.Relative);
                
                #line 1 ""MainWindow.xaml""
                System.Windows.Application.LoadComponent(this, resourceLocater);
                
                #line default
                #line hidden
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.CodeDom.Compiler.GeneratedCodeAttribute(""PresentationBuildTasks"", ""4.0.0.0"")]
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Design"", ""CA1033:InterfaceMethodsShouldBeCallableByChildTypes"")]
            [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Maintainability"", ""CA1502:AvoidExcessiveComplexity"")]
            [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1800:DoNotCastUnnecessarily"")]
            void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
                switch (connectionId)
                {
                case 1:
                this.Foo = ((System.Windows.Controls.Grid)(target));
                return;
                }
                this._contentLoaded = true;
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        [Description("Issue 14")]
        public void FieldWithNamingViolation_ClassHasGeneratedCodeAttribute_Ignored()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""16.0.0.0"")]
        class TypeName
        {   
            public string _myField;
        }
    }";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        [Description("Issue 14")]
        public void FieldWithNamingViolation_FieldHasGeneratedCodeAttribute_Ignored()
        {
            var test = @"
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
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""16.0.0.0"")]
            public string _myField;
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeFixes.NamingFieldPascalUnderscore();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzers.NamingFieldPascalUnderscore();
        }
    }
}
