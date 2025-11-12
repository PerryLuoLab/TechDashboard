using System;
using System.Linq;
using System.Windows;
using TechDashboard.Services.Interfaces;

namespace TechDashboard.Services
{
    /// <summary>
    /// Implementation of theme management service
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ILocalizationService _localizationService;
        private string _currentTheme = "Dark";
        private readonly string[] _availableThemes = { "Dark", "Light", "BlueTech" };

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

            // Normalize theme name
            string themeFileName = themeName;
            if (!themeFileName.EndsWith("Theme", StringComparison.OrdinalIgnoreCase))
            {
                themeFileName = themeName + "Theme";
            }

            var themePath = $"Themes/{themeFileName}.xaml";

            try
            {
                // Remove old theme dictionaries
                var toRemove = Application.Current.Resources.MergedDictionaries
                    .Where(d => d.Source != null && d.Source.OriginalString.Contains("Themes/"))
                    .ToList();

                foreach (var dict in toRemove)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dict);
                }

                // Add new theme
                var newTheme = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(newTheme);

                _currentTheme = themeName;
                
                System.Diagnostics.Debug.WriteLine($"? Theme changed to: {themeName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Theme change failed: {ex.Message}");
                
                // Fallback to default theme
                try
                {
                    var defaultDict = new ResourceDictionary { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) };
                    Application.Current.Resources.MergedDictionaries.Add(defaultDict);
                    _currentTheme = "Dark";
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"? Fallback theme failed: {fallbackEx.Message}");
                }
            }
        }

        /// <inheritdoc/>
        public string[] GetAvailableThemes()
        {
            return _availableThemes;
        }

        /// <inheritdoc/>
        public string GetThemeDisplayName(string themeName)
        {
            var themeKey = themeName switch
            {
                "Dark" => "Status_Theme_Dark",
                "Light" => "Status_Theme_Light",
                "BlueTech" => "Status_Theme_BlueTech",
                _ => null
            };

            return themeKey != null 
                ? _localizationService.GetString(themeKey) 
                : themeName;
        }
    }
}
