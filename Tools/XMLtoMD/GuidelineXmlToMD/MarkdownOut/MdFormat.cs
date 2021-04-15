namespace MarkdownOut {

    /// <summary>
    /// Specifies a Markdown format to apply to a line or block of text. Formats are applied with a
    /// prefix string and, unlike Markdown styles (see <see cref="MdStyle"/>), cannot be selectively
    /// applied to substrings.
    /// </summary>
    public enum MdFormat {

        /// <summary>
        /// No text format.
        /// </summary>
        None,

        /// <summary>
        /// Heading 1 text format (inserts the string <c>"# "</c> in front of text).
        /// </summary>
        Heading1,

        /// <summary>
        /// Heading 2 text format (inserts the string <c>"## "</c> in front of text).
        /// </summary>
        Heading2,

        /// <summary>
        /// Heading 3 text format (inserts the string <c>"### "</c> in front of text).
        /// </summary>
        Heading3,

        /// <summary>
        /// Heading 4 text format (inserts the string <c>"#### "</c> in front of text).
        /// </summary>
        Heading4,

        /// <summary>
        /// Heading 5 text format (inserts the string <c>"##### "</c> in front of text).
        /// </summary>
        Heading5,

        /// <summary>
        /// Heading 6 text format (inserts the string <c>"###### "</c> in front of text).
        /// </summary>
        Heading6,

        /// <summary>
        /// Quote text format (inserts the string <c>"> "</c> in front of text).
        /// </summary>
        Quote,

        /// <summary>
        /// Unordered list item text format (inserts the string <c>"- "</c> in front of text).
        /// </summary>
        UnorderedListItem,

        /// <summary>
        /// Ordered list item text format (inserts the string <c>"1. "</c> in front of text).
        /// </summary>
        OrderedListItem,

        /// <summary>
        /// Creates a link to the heading that matches text to write (formats the text as [Foo](#foo)).
        /// </summary>
        InternalLink,
    }
}
