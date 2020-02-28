using System.Runtime.CompilerServices;

namespace IntelliTect.Analyzer
{
#pragma warning disable CA1055 // Uri return values should not be strings
    
    public static class DiagnosticUrlBuilder
    {
        private const string BaseUrl = "https://github.com/IntelliTect/CodingStandards/wiki/";

        /// <summary>
        /// Get the full diagnostic help url
        /// </summary>
        /// <param name="analyzerBlock">Current analyzer block</param>
        /// <param name="intlCode">The intl error code</param>
        /// <returns>Full url linking to wiki </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUrl(AnalyzerBlock analyzerBlock, string intlCode)
        {
            return BaseUrl + analyzerBlock.GetDescription() + $"#{intlCode}";
        }
    }

#pragma warning restore CA1055 // Uri return values should not be strings
}
