namespace IntelliTectAnalyzer
{
    public enum AnalyzerBlock
    {
        None,
        [Description("INTL00XX.Naming")]
        Naming,
        [Description("INTL01XX.Formatting")]
        Formatting,
        [Description("INTL02XX.Reliability")]
        Reliability,
        [Description("INTL03XX.Performance")]
        Performance
    }
}
