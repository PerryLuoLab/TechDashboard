using System;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace TechDashboard.Helpers
{
    /// <summary>
    /// Helper class for localization using WPFLocalizeExtension
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Gets the current culture
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get => LocalizeDictionary.Instance.Culture;
            set
            {
                if (value != null)
                {
                    LocalizeDictionary.Instance.Culture = value;
                }
            }
        }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">Resource key (e.g., "Nav_Dashboard")</param>
        /// <returns>Localized string or key if not found</returns>
        public static string GetString(string key)
        {
            return GetString("TechDashboard", "Strings", key);
        }

        /// <summary>
        /// Gets a localized string with full parameters
        /// </summary>
        /// <param name="assembly">Assembly name (e.g., "TechDashboard")</param>
        /// <param name="dictionary">Resource dictionary name (e.g., "Strings")</param>
        /// <param name="key">Resource key</param>
        /// <returns>Localized string or key if not found</returns>
        public static string GetString(string assembly, string dictionary, string key)
        {
            try
            {
                var localizedObject = LocalizeDictionary.Instance.GetLocalizedObject(
                    assembly,
                    dictionary,
                    key,
                    LocalizeDictionary.Instance.Culture
                );

                return localizedObject?.ToString() ?? key;
            }
            catch
            {
                return key;
            }
        }

        /// <summary>
        /// Gets a formatted localized string
        /// </summary>
        /// <param name="key">Resource key containing format placeholders (e.g., "Welcome, {0}!")</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Formatted localized string</returns>
        public static string GetFormattedString(string key, params object[] args)
        {
            try
            {
                var template = GetString(key);
                return string.Format(template, args);
            }
            catch
            {
                return key;
            }
        }

        /// <summary>
        /// Changes the application language
        /// </summary>
        /// <param name="cultureCode">Culture code (e.g., "en-US", "zh-CN")</param>
        public static void ChangeLanguage(string cultureCode)
        {
            try
            {
                var culture = new CultureInfo(cultureCode);
                LocalizeDictionary.Instance.Culture = culture;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to change language to {cultureCode}: {ex.Message}");
                // Fallback to English
                LocalizeDictionary.Instance.Culture = new CultureInfo("en-US");
            }
        }

        /// <summary>
        /// Gets available cultures based on existing resource files
        /// </summary>
        public static CultureInfo[] GetAvailableCultures()
        {
            return new[]
            {
                new CultureInfo("en-US"), // English
                new CultureInfo("zh-CN"), // Simplified Chinese
                new CultureInfo("zh-TW"), // Traditional Chinese
                new CultureInfo("ko-KR"), // Korean
                new CultureInfo("ja-JP")  // Japanese
            };
        }

        /// <summary>
        /// Gets the display name for a language code
        /// </summary>
        /// <param name="cultureCode">Culture code (e.g., "zh-CN")</param>
        /// <returns>Display name (e.g., "简体中文")</returns>
        public static string GetLanguageDisplayName(string cultureCode)
        {
            return cultureCode switch
            {
                "en-US" => "English",
                "zh-CN" => "简体中文",
                "zh-TW" => "繁體中文",
                "ko-KR" => "???",
                "ja-JP" => "日本語",
                _ => cultureCode
            };
        }
    }
}
