using TechDashboard.Core.Constants;

namespace TechDashboard.Options
{
    /// <summary>
    /// Localization configuration options (single source of truth driven by LanguageConstants).
    /// Extendable for future environment/user settings.
    /// </summary>
    public class LocalizationOptions
    {
        /// <summary>Assembly name containing the resx resources.</summary>
        public string AssemblyName { get; set; } = "TechDashboard";

        /// <summary>Base dictionary name (without culture suffix).</summary>
        public string DictionaryName { get; set; } = "Strings";

        /// <summary>Default UI culture when no preference is stored.</summary>
        public string DefaultCulture { get; set; } = LanguageConstants.DefaultLanguage;

        /// <summary>Available culture codes exposed to UI selection.</summary>
        public string[] AvailableCultures { get; set; } = LanguageConstants.GetAllCultureCodes();

        // --- Optional future flags (placeholders) ---
        /// <summary>Enable metrics collection for missing keys / usage stats.</summary>
        public bool EnableMetrics { get; set; } = false;

        /// <summary>If true, only load selected culture dictionaries eagerly.</summary>
        public bool LazyLoad { get; set; } = false;

        /// <summary>Fallback chain (first item is highest priority). If empty, falls back to DefaultCulture.</summary>
        public string[] FallbackChain { get; set; } = new string[0];
    }
}
