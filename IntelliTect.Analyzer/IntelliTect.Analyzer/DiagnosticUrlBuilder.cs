using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace IntelliTect.Analyzer
{
    public static class DiagnosticUrlBuilder
    {
        private const string BaseUrl = "https://github.com/IntelliTect/CodingGuidelines";
        private static readonly Regex _HyphenateRegex = new(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// Get the full diagnostic help url
        /// </summary>
        /// <param name="title">IntelliTect analyzer title</param>
        /// <param name="diagnosticId">The IntelliTect error code</param>
        /// <returns>Full url linking to wiki </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUrl(string title, string diagnosticId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new System.ArgumentException("title cannot be empty", nameof(title));
            if (string.IsNullOrWhiteSpace(diagnosticId))
                throw new System.ArgumentException("diagnostic ID cannot be empty", nameof(diagnosticId));

            string hyphenatedTitle = _HyphenateRegex.Replace(title, "-");

            return BaseUrl + $"#{diagnosticId.ToUpperInvariant()}" + $"---{hyphenatedTitle.ToUpperInvariant()}";
        }
    }
}
