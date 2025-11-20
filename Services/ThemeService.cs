using System;
using System.Linq;
using System.Windows;
using TechDashboard.Services.Interfaces;
using TechDashboard.Core.Constants;
using Microsoft.Extensions.Logging;

namespace TechDashboard.Services
{
    /// <summary>
    /// Implementation of theme management service
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<ThemeService> _logger;
        private string _currentTheme = ThemeConstants.DefaultTheme;

        public ThemeService(ILocalizationService localizationService, ILogger<ThemeService> logger)
        {
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public string CurrentTheme => _currentTheme;

        /// <inheritdoc/>
        public void ApplyTheme(string themeName)
        {
            if (Application.Current == null)
            {
                _logger.LogWarning("Cannot apply theme: Application.Current is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(themeName))
            {
                _logger.LogWarning("Cannot apply theme: theme name was empty");
                return;
            }

            // Normalize theme name to one of defined constants
            if (!ThemeConstants.IsValidTheme(themeName))
            {
                _logger.LogWarning("Unknown theme '{Theme}', falling back to default '{Default}'", themeName, ThemeConstants.DefaultTheme);
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

                _logger.LogInformation("Theme changed to {Theme}", themeName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Theme change failed for {Theme}", themeName);
                // Fallback
                try
                {
                    var fallbackPath = ThemeConstants.GetThemeResourcePath(ThemeConstants.DefaultTheme);
                    var defaultDict = new ResourceDictionary { Source = new Uri(fallbackPath, UriKind.Relative) };
                    Application.Current.Resources.MergedDictionaries.Add(defaultDict);
                    _currentTheme = ThemeConstants.DefaultTheme;
                    _logger.LogWarning("Fallback theme applied: {Theme}", _currentTheme);
                }
                catch (Exception fallbackEx)
                {
                    _logger.LogCritical(fallbackEx, "Fallback theme application failed");
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
