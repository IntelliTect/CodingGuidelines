using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntelliTect.Analyzer.Tests
{
    [TestClass]
    public class DiagnosticUriBuilderTests
    {
        [TestMethod]
        [DataRow("Fields _PascalCase", "intl0001",
            "https://github.com/IntelliTect/CodingGuidelines#intl0001---fields-_pascalcase")]
        [DataRow("Async void methods", "INTL0201",
            "https://github.com/IntelliTect/CodingGuidelines#intl0201---async-void-methods")]
        public void GetUrl_GivenBlock00XXCode_ProperlyBuildsUrl(string title, string diagnosticId,
             string expected)
        {
            string actual = DiagnosticUrlBuilder.GetUrl(title, diagnosticId);

            Assert.IsTrue(string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase),
                $"'{expected}' does not equal '{actual}'");
        }
    }
}
