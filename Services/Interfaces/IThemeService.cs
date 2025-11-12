namespace TechDashboard.Services.Interfaces
{
    /// <summary>
    /// Interface for theme management service
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Gets the current theme name
        /// </summary>
        string CurrentTheme { get; }

        /// <summary>
        /// Applies a theme to the application
        /// </summary>
        /// <param name="themeName">Theme name (e.g., "Dark", "Light", "BlueTech")</param>
        void ApplyTheme(string themeName);

        /// <summary>
        /// Gets available theme names
        /// </summary>
        /// <returns>Array of available theme names</returns>
        string[] GetAvailableThemes();

        /// <summary>
        /// Gets the display name for a theme
        /// </summary>
        /// <param name="themeName">Theme name</param>
        /// <returns>Localized display name</returns>
        string GetThemeDisplayName(string themeName);
    }
}
