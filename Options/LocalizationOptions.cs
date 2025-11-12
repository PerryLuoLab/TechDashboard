namespace TechDashboard.Options
{
    /// <summary>
    /// Localization configuration options
    /// </summary>
    public class LocalizationOptions
    {
        /// <summary>
        /// Assembly name containing the resources
        /// Default: "TechDashboard"
        /// </summary>
        public string AssemblyName { get; set; } = "TechDashboard";

        /// <summary>
        /// Resource dictionary name
        /// Default: "Strings"
        /// </summary>
        public string DictionaryName { get; set; } = "Strings";

        /// <summary>
        /// Default culture code
        /// Default: "en-US"
        /// </summary>
        public string DefaultCulture { get; set; } = "en-US";

        /// <summary>
        /// Available culture codes
        /// </summary>
        public string[] AvailableCultures { get; set; } = new[]
        {
            "en-US", // English
            "zh-CN", // Simplified Chinese
            "zh-TW", // Traditional Chinese
            "ko-KR", // Korean
            "ja-JP"  // Japanese
        };
    }
}
