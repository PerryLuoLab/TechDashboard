using System;
using System.Linq;
using System.Windows;

namespace TechDashboard
{
    public partial class App : Application
    {
        public static void ApplyTheme(string themeName)
        {
            if (Application.Current == null) return;

            // 处理主题名称
            string themeFileName = themeName;
            if (!themeFileName.EndsWith("Theme", StringComparison.OrdinalIgnoreCase))
            {
                themeFileName = themeName + "Theme";
            }

            var themePath = $"Themes/{themeFileName}.xaml";

            // 移除所有现有主题
            var toRemove = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Themes/")).ToList();

            foreach (var d in toRemove)
            {
                Application.Current.Resources.MergedDictionaries.Remove(d);
            }

            // 添加新主题
            try
            {
                var dict = new ResourceDictionary() { Source = new Uri(themePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch
            {
                // 如果主题文件不存在，回退到默认主题
                var defaultDict = new ResourceDictionary() { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(defaultDict);
            }
        }

        public static void ApplyLanguage(string languageCode)
        {
            if (Application.Current == null) return;

            var languagePath = $"Languages/{languageCode}.xaml";

            // 移除所有现有语言包
            var toRemove = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source != null && d.Source.OriginalString.Contains("Languages/")).ToList();

            foreach (var d in toRemove)
            {
                Application.Current.Resources.MergedDictionaries.Remove(d);
            }

            // 添加新语言包
            try
            {
                var dict = new ResourceDictionary() { Source = new Uri(languagePath, UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch
            {
                // 如果语言文件不存在，回退到英语
                var defaultDict = new ResourceDictionary() { Source = new Uri("Languages/en-US.xaml", UriKind.Relative) };
                Application.Current.Resources.MergedDictionaries.Add(defaultDict);
            }
        }
    }
}