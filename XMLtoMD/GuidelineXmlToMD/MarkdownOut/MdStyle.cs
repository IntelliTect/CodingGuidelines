namespace MarkdownOut {

    /// <summary>
    /// Specifies a Markdown style to apply to a string of text. Styles are applied by wrapping text
    /// with a special string on each side and can be selectively applied to substrings.
    /// </summary>
    public enum MdStyle {

        /// <summary>
        /// No text styling.
        /// </summary>
        None,

        /// <summary>
        /// Italic text styling (surrounds text with a single <c>*</c> character on each side).
        /// </summary>
        Italic,

        /// <summary>
        /// Bold text styling (surrounds text with two <c>*</c> characters on each side).
        /// </summary>
        Bold,

        /// <summary>
        /// Bold italic text styling (surrounds text with three <c>*</c> characters on each side).
        /// </summary>
        BoldItalic,

        /// <summary>
        /// Code text styling (surrounds text with a single <c>`</c> character on each side).
        /// </summary>
        Code,

        /// <summary>
        /// Strike-through text styling (surrounds text with two <c>~</c> characters on each side).
        /// This style may not be supported by all Markdown parsers.
        /// </summary>
        StrikeThrough,
    }
}