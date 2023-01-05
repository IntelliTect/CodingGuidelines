using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntelliTect.Analyzer.Integration.Tests
{
    [TestClass]
    public class AnalyzerTests
    {
        static AnalyzerTests()
        {
            MSBuildLocatorInitializer.Initialize();
        }

        [TestMethod]
        public async Task RunOnSelf()
        {
            await ProcessProject(new FileInfo(Path.Combine("..", "..", "..", "..", "IntelliTect.Analyzer", "IntelliTect.Analyzer.csproj")))
                .ConfigureAwait(false);
        }

        public static async Task ProcessProject(FileInfo projectFile)
        {
            if (projectFile is null)
            {
                throw new ArgumentNullException(nameof(projectFile));
            }

            using var workspace = MSBuildWorkspace.Create();
            Project project = await workspace.OpenProjectAsync(projectFile.FullName).ConfigureAwait(false);

            CompilationWithAnalyzers compilationWithAnalyzers = (await project.GetCompilationAsync().ConfigureAwait(false))
                .WithAnalyzers(ImmutableArray.Create(GetAnalyzers().ToArray()));

            ImmutableArray<Diagnostic> diags = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().ConfigureAwait(false);
            foreach (Diagnostic diag in diags)
            {
                Assert.Fail(diag.ToString());
            }
        }

        private static IEnumerable<DiagnosticAnalyzer> GetAnalyzers()
        {
            Assembly assembly = typeof(AnalyzerBlock).Assembly;
            foreach (Type analyzer in assembly.GetTypes().Where(x => typeof(DiagnosticAnalyzer).IsAssignableFrom(x)))
            {
                if (analyzer.IsAbstract) continue;

                if (Activator.CreateInstance(analyzer) is DiagnosticAnalyzer analyzerInstance)
                {
                    yield return analyzerInstance;
                }
            }
        }

    }
}
