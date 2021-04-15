﻿namespace MarkdownOut {

    /// <summary>
    /// A container for extension methods related to Markdown styling and formatting.
    /// </summary>
    public static class MdExtensions {

        /// <summary>
        /// Styles one or more substrings of the provided string using the specified
        /// <see cref="MdStyle"/>.
        /// </summary>
        /// <param name="str">The string containing the substring to style.</param>
        /// <param name="substring">The substring to style.</param>
        /// <param name="style">The Markdown style to apply.</param>
        /// <param name="firstOnly">
        /// If true, only the first occurrence of the substring is styled; otherwise, all
        /// occurrences of the substring are styled.
        /// </param>
        /// <returns>
        /// The selectively styled string. If <paramref name="str"/> does not contain any
        /// occurrences of <paramref name="substring"/>, it is returned unchanged.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static string StyleSubstring(this string str, string substring, MdStyle style,
                                            bool firstOnly = false) {
            if (string.IsNullOrEmpty(str))
            {
                throw new System.ArgumentException("cannot stylize empty string", nameof(str));
            }

            if (string.IsNullOrEmpty(substring))
            {
                throw new System.ArgumentException("cannot stylize empty string", nameof(str));
            }

            if (!firstOnly) {
                return str?.Replace(substring, MdText.Style(substring, style), System.StringComparison.Ordinal);
            }
            int pos = str.IndexOf(substring, System.StringComparison.Ordinal);
            if (pos < 0) {
                return str;
            }
            return str.Substring(0, pos) + MdText.Style(substring, style)
                   + str.Substring(pos + substring.Length);
        }
    }
}
