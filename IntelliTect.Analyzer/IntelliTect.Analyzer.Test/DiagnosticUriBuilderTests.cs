using System;
using System.Diagnostics;
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

        [TestMethod]
        [Description("GetUrl allocates a new Regex on every call — performance regression test")]
        public void GetUrl_CalledRepeatedly_DoesNotAllocateExcessively()
        {
            // Warm up
            DiagnosticUrlBuilder.GetUrl("Test Title", "INTL0001");

            long before = GC.GetAllocatedBytesForCurrentThread();

            const int iterations = 1000;
            for (int i = 0; i < iterations; i++)
            {
                DiagnosticUrlBuilder.GetUrl("Test Title", "INTL0001");
            }

            long after = GC.GetAllocatedBytesForCurrentThread();
            long bytesPerCall = (after - before) / iterations;

            // A cached Regex or simple string.Replace should allocate ~200-400 bytes per call (strings only).
            // A new Regex() per call allocates ~2000+ bytes. Threshold at 500 to catch the Regex allocation.
            Assert.IsTrue(bytesPerCall < 500,
                $"GetUrl allocated ~{bytesPerCall} bytes/call, suggesting a new Regex is created each time. " +
                $"Expected < 500 bytes/call with a cached approach.");
        }

        [TestMethod]
        [Description("Titles with multiple whitespace types should be hyphenated correctly")]
        public void GetUrl_TitleWithTabsAndMultipleSpaces_HyphenatesCorrectly()
        {
            string actual = DiagnosticUrlBuilder.GetUrl("Fields  Multiple\tSpaces", "INTL9999");

            Assert.IsTrue(actual.Contains("FIELDS-MULTIPLE-SPACES", StringComparison.OrdinalIgnoreCase),
                $"Expected consecutive whitespace collapsed to a single hyphen but got: '{actual}'");
        }
    }
}
