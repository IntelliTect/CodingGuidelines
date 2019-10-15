using System;

namespace IntelliTectAnalyzer
{
#pragma warning disable CA1055 // Uri return values should not be strings
    
    public static class DiagnosticUrlBuilder//https://github.com/IntelliTect/CodingStandards/wiki/03XX.Performance#intl0301
    {
        private const string BaseUrl = "https://github.com/IntelliTect/CodingStandards/wiki";

        private const string UrlSeparatorCharacter = "/";
        
        /// <summary>
        /// Get the full diagnostic help url
        /// </summary>
        /// <param name="analyzerBlock">Current analyzer block</param>
        /// <param name="intlCode">The intl error code</param>
        /// <returns></returns>
        public static string GetUrl(AnalyzerBlock analyzerBlock, int intlCode)
        {
            return BaseUrl + UrlSeparatorCharacter + analyzerBlock.GetDescription() + $"#intl{intlCode}";
        }
    }

#pragma warning restore CA1055 // Uri return values should not be strings
}
