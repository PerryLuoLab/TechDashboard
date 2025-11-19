namespace TechDashboard.Core.Constants
{
    /// <summary>
    /// Language-related constants and configuration values.
    /// </summary>
    public static class LanguageConstants
    {
        /// <summary>
        /// Available language culture codes
        /// </summary>
        public static class CultureCodes
        {
            /// <summary>
            /// English (United States)
            /// </summary>
            public const string English = "en-US";

            /// <summary>
            /// Simplified Chinese (China)
            /// </summary>
            public const string SimplifiedChinese = "zh-CN";

            /// <summary>
            /// Traditional Chinese (Taiwan)
            /// </summary>
            public const string TraditionalChinese = "zh-TW";

            /// <summary>
            /// Korean (South Korea)
            /// </summary>
            public const string Korean = "ko-KR";

            /// <summary>
            /// Japanese (Japan)
            /// </summary>
            public const string Japanese = "ja-JP";
        }

        /// <summary>
        /// Display names for languages
        /// </summary>
        public static class DisplayNames
        {
            /// <summary>
            /// English display name
            /// </summary>
            public const string English = "English";

            /// <summary>
            /// Simplified Chinese display name
            /// </summary>
            public const string SimplifiedChinese = "简体中文";

            /// <summary>
            /// Traditional Chinese display name
            /// </summary>
            public const string TraditionalChinese = "繁體中文";

            /// <summary>
            /// Korean display name
            /// </summary>
            public const string Korean = "한국어";

            /// <summary>
            /// Japanese display name
            /// </summary>
            public const string Japanese = "日本語";
        }

        /// <summary>
        /// Default language to use when no preference is set
        /// </summary>
        public const string DefaultLanguage = CultureCodes.English;

        /// <summary>
        /// Gets the display name for a given culture code
        /// </summary>
        /// <param name="cultureCode">The culture code (e.g., "en-US", "zh-CN")</param>
        /// <returns>The display name for the language</returns>
        public static string GetDisplayName(string cultureCode)
        {
            return cultureCode switch
            {
                CultureCodes.English => DisplayNames.English,
                CultureCodes.SimplifiedChinese => DisplayNames.SimplifiedChinese,
                CultureCodes.TraditionalChinese => DisplayNames.TraditionalChinese,
                CultureCodes.Korean => DisplayNames.Korean,
                CultureCodes.Japanese => DisplayNames.Japanese,
                _ => DisplayNames.English
            };
        }

        /// <summary>
        /// Gets all available culture codes
        /// </summary>
        /// <returns>Array of culture codes</returns>
        public static string[] GetAllCultureCodes()
        {
            return new[]
            {
                CultureCodes.English,
                CultureCodes.SimplifiedChinese,
                CultureCodes.TraditionalChinese,
                CultureCodes.Korean,
                CultureCodes.Japanese
            };
        }

        /// <summary>
        /// Validates if a culture code is supported
        /// </summary>
        /// <param name="cultureCode">The culture code to validate</param>
        /// <returns>True if the culture is supported, false otherwise</returns>
        public static bool IsSupported(string cultureCode)
        {
            return cultureCode switch
            {
                CultureCodes.English => true,
                CultureCodes.SimplifiedChinese => true,
                CultureCodes.TraditionalChinese => true,
                CultureCodes.Korean => true,
                CultureCodes.Japanese => true,
                _ => false
            };
        }
    }
}
