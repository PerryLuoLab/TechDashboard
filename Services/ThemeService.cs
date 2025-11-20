using System;
using System.Linq;
using System.Windows;
using TechDashboard.Services.Interfaces;
using TechDashboard.Core.Constants;

namespace TechDashboard.Services
{
    /// <summary>
    /// Implementation of theme management service
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ILocalizationService _localizationService;
        private string _currentTheme = ThemeConstants.DefaultTheme;

        public ThemeService(ILocalizationService localizationService)
        {
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        /// <inheritdoc/>
        public string CurrentTheme => _currentTheme;

        /// <inheritdoc/>
        public void ApplyTheme(string themeName)
        {
            if (Application.Current == null)
            {
                System.Diagnostics.Debug.WriteLine("Cannot apply theme: Application.Current is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(themeName))
            {
                System.Diagnostics.Debug.WriteLine("Cannot apply theme: Theme name is null or empty");
                return;
            }

            // Normalize theme name to one of defined constants
            if (!ThemeConstants.IsValidTheme(themeName))
            {
                System.Diagnostics.Debug.WriteLine($"Unknown theme '{themeName}', falling back to default '{ThemeConstants.DefaultTheme}'");
                themeName = ThemeConstants.DefaultTheme;
            }

            var themePath = ThemeConstants.GetThemeResourcePath(themeName);

            try
            {
                var toRemove = Application.Current.Resources.MergedDictionaries
                    .Where(d => d.Source != null && d.Source.OriginalString.Contains("Themes/"))
                    .ToList();

                foreach (var dict in toRemove)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dict);
                }

                var newTheme = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(newTheme);

                _currentTheme = themeName;

                System.Diagnostics.Debug.WriteLine($"[ThemeService] Theme changed to: {themeName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ThemeService] Theme change failed: {ex.Message}");
                // Fallback
                try
                {
                    var fallbackPath = ThemeConstants.GetThemeResourcePath(ThemeConstants.DefaultTheme);
                    var defaultDict = new ResourceDictionary { Source = new Uri(fallbackPath, UriKind.Relative) };
                    Application.Current.Resources.MergedDictionaries.Add(defaultDict);
                    _currentTheme = ThemeConstants.DefaultTheme;
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[ThemeService] Fallback theme failed: {fallbackEx.Message}");
                }
            }
        }

        /// <inheritdoc/>
        public string[] GetAvailableThemes() => ThemeConstants.OrderedThemes;

        /// <inheritdoc/>
        public string GetThemeDisplayName(string themeName)
        {
            if (ThemeConstants.ThemeDisplayLocalizationKeys.TryGetValue(themeName, out var key))
            {
                return _localizationService.GetString(key);
            }
            return themeName;
        }
    }
}
