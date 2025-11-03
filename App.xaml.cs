using System;
using System.Linq;
using System.Windows;

namespace TechDashboard
{
    public partial class App : Application
    {
        public static void ApplyTheme(string themeFileName)
        {
            if (Application.Current == null) return;
            var themePath = $"Themes/{themeFileName}Theme.xaml";

            var existing = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.EndsWith($"{themeFileName}Theme.xaml", StringComparison.OrdinalIgnoreCase));
            if (existing != null) return;

            var toRemove = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Themes/")).ToList();

            foreach (var d in toRemove) Application.Current.Resources.MergedDictionaries.Remove(d);

            var dict = new ResourceDictionary() { Source = new Uri(themePath, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
