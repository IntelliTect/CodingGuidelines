using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace TestHelper
{
    /// <summary>
    /// Registry of supported test frameworks for analyzer testing.
    /// Used both to inject real assembly references into in-memory Roslyn compilations
    /// and as data source for parameterized tests via [DynamicData].
    ///
    /// To add a new test framework: add one entry to <see cref="All"/>,
    /// add the NuGet package to Directory.Packages.props and the test .csproj
    /// (with IncludeAssets="compile" PrivateAssets="all"), and add the namespace
    /// to <see cref="IntelliTect.Analyzer.Analyzers.NamingMethodPascal"/>'s namespace list.
    /// </summary>
    public static class TestFrameworkReferences
    {
        public record TestFramework(
            string Name,
            string TestAttribute,
            string UsingDirective,
            MetadataReference Reference);

        public static IReadOnlyList<TestFramework> All { get; } =
        [
            new("MSTest",
                "[TestMethod]",
                "using Microsoft.VisualStudio.TestTools.UnitTesting;",
                MetadataReference.CreateFromFile(
                    typeof(Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute).Assembly.Location)),
            new("xUnit",
                "[Fact]",
                "using Xunit;",
                MetadataReference.CreateFromFile(
                    typeof(Xunit.FactAttribute).Assembly.Location)),
            new("NUnit",
                "[Test]",
                "using NUnit.Framework;",
                MetadataReference.CreateFromFile(
                    typeof(NUnit.Framework.TestAttribute).Assembly.Location)),
            new("TUnit",
                "[Test]",
                "using TUnit.Core;",
                MetadataReference.CreateFromFile(
                    typeof(TUnit.Core.TestAttribute).Assembly.Location)),
        ];

        /// <summary>All test framework assembly references, for use in <see cref="DiagnosticVerifier"/>.</summary>
        public static MetadataReference[] AllReferences =>
            [.. All.Select(f => f.Reference)];
    }
}
