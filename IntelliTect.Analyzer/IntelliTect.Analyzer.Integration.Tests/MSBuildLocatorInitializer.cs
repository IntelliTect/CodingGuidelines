namespace IntelliTect.Analyzer.Integration.Tests
{
    internal static class MSBuildLocatorInitializer
    {
        static MSBuildLocatorInitializer()
        {
            VisualStudioInstance =
                Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
        }
        public static void Initialize()
        {
        }
        public static Microsoft.Build.Locator.VisualStudioInstance VisualStudioInstance { get; }

    }
}
