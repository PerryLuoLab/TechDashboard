using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace TechDashboard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize WPFLocalizeExtension
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo("en-US");
            
            // Debug output
            System.Diagnostics.Debug.WriteLine($"? LocalizeDictionary initialized");
            System.Diagnostics.Debug.WriteLine($"  Culture: {LocalizeDictionary.Instance.Culture.Name}");
            System.Diagnostics.Debug.WriteLine($"  Assembly: TechDashboard");
            System.Diagnostics.Debug.WriteLine($"  Dictionary: Strings");
            
            // Test localization
            var testKey = LocalizeDictionary.Instance.GetLocalizedObject("TechDashboard", "Strings", "Menu_File", LocalizeDictionary.Instance.Culture);
            System.Diagnostics.Debug.WriteLine($"  Test key 'Menu_File' = '{testKey}'");
        }

        public static void ApplyTheme(string themeName)
        {
            if (Application.Current == null) return;

            // Normalize theme name
            string themeFileName = themeName;
            if (!themeFileName.EndsWith("Theme", StringComparison.OrdinalIgnoreCase))
            {
                themeFileName = themeName + "Theme";
            }

            var themePath = $"Themes/{themeFileName}.xaml";

            // Remove old theme dictionaries
            var toRemove = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Themes/")).ToList();

            foreach (var d in toRemove)
            {
                Application.Current.Resources.MergedDictionaries.Remove(d);
            }

            // Add new theme
            try
            {
                var dict = new ResourceDictionary() { Source = new Uri(themePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(dict);
                System.Diagnostics.Debug.WriteLine($"? Theme changed to: {themeName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Theme change failed: {ex.Message}");
                // If theme file doesn't exist, fallback to default theme
                var defaultDict = new ResourceDictionary() { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(defaultDict);
            }
        }

        public static void ApplyLanguage(string languageCode)
        {
            if (Application.Current == null) return;

            try
            {
                var oldCulture = LocalizeDictionary.Instance.Culture.Name;
                System.Diagnostics.Debug.WriteLine($"¡ú Changing language from {oldCulture} to {languageCode}");
                
                // Use WPFLocalizeExtension to change culture
                var newCulture = new CultureInfo(languageCode);
                LocalizeDictionary.Instance.Culture = newCulture;
                
                System.Diagnostics.Debug.WriteLine($"? Language changed successfully");
                System.Diagnostics.Debug.WriteLine($"  New culture: {LocalizeDictionary.Instance.Culture.Name}");
                
                // Force UI update
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Trigger property changed on all bindings
                    foreach (Window window in Application.Current.Windows)
                    {
                        window.Language = System.Windows.Markup.XmlLanguage.GetLanguage(newCulture.IetfLanguageTag);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? Error changing language: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"  Stack: {ex.StackTrace}");
                
                // If language code is invalid, fallback to English
                LocalizeDictionary.Instance.Culture = new CultureInfo("en-US");
            }
        }
    }
}