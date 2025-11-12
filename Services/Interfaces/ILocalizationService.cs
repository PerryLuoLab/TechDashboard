using System.Globalization;

namespace TechDashboard.Services.Interfaces
{
    /// <summary>
    /// Interface for localization service
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Gets or sets the current culture
        /// </summary>
        CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">Resource key</param>
        /// <returns>Localized string or key if not found</returns>
        string GetString(string key);

        /// <summary>
        /// Gets a formatted localized string
        /// </summary>
        /// <param name="key">Resource key containing format placeholders</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Formatted localized string</returns>
        string GetFormattedString(string key, params object[] args);

        /// <summary>
        /// Changes the application language
        /// </summary>
        /// <param name="cultureCode">Culture code (e.g., "en-US", "zh-CN")</param>
        void ChangeLanguage(string cultureCode);

        /// <summary>
        /// Gets available cultures
        /// </summary>
        /// <returns>Array of available cultures</returns>
        CultureInfo[] GetAvailableCultures();

        /// <summary>
        /// Gets the display name for a language code
        /// </summary>
        /// <param name="cultureCode">Culture code</param>
        /// <returns>Display name</returns>
        string GetLanguageDisplayName(string cultureCode);
    }
}
