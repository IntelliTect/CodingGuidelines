using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntelliTectAnalyzer.Tests
{
    [TestClass]
    public class DiagnosticUriBuilderTests
    {
        [TestMethod]
        [DataRow(AnalyzerBlock.Naming, "intl0001",
            "https://github.com/IntelliTect/CodingStandards/wiki/00XX.Naming#intl0001")]
        [DataRow(AnalyzerBlock.Performance, "intl0301",
            "https://github.com/IntelliTect/CodingStandards/wiki/03XX.Performance#intl0301")]
        public void GetUrl_GivenBlock00XXCode_ProperlyBuildsUrl(AnalyzerBlock analyzerBlock, string code,
            string expected)
        {
            string actual = DiagnosticUrlBuilder.GetUrl(analyzerBlock, code);
            
            Assert.IsTrue(string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase),
                $"'{expected}' does not equal '{actual}'");
        }
    }
}
