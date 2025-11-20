using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TechDashboard.Tools
{
    /// <summary>
    /// Helper class for working with icon constants. Design-time / tools usage.
    /// </summary>
    public static class IconHelper
    {
        public static IEnumerable<string> GetCategories()
        {
            return typeof(Core.Constants.IconConstants)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .Select(t => t.Name);
        }

        public static Dictionary<string, string> GetIconsInCategory(string categoryName)
        {
            var categoryType = typeof(Core.Constants.IconConstants)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(t => t.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (categoryType == null)
                return new Dictionary<string, string>();

            return categoryType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .ToDictionary(f => f.Name, f => f.GetValue(null)?.ToString() ?? string.Empty);
        }

        public static Dictionary<string, string> GetAllIcons()
        {
            var result = new Dictionary<string, string>();
            foreach (var category in GetCategories())
            {
                var icons = GetIconsInCategory(category);
                foreach (var icon in icons)
                {
                    result[$"{category}.{icon.Key}"] = icon.Value;
                }
            }
            return result;
        }

        public static Dictionary<string, string> SearchIcons(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new Dictionary<string, string>();
            var allIcons = GetAllIcons();
            return allIcons
                .Where(kvp => kvp.Key.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static string GetIcon(string iconName)
        {
            if (string.IsNullOrWhiteSpace(iconName))
                return string.Empty;
            var commonIcons = GetIconsInCategory("Common");
            if (commonIcons.TryGetValue(iconName, out var icon)) return icon;
            var navIcons = GetIconsInCategory("Navigation");
            if (navIcons.TryGetValue(iconName, out icon)) return icon;
            var statusIcons = GetIconsInCategory("Status");
            if (statusIcons.TryGetValue(iconName, out icon)) return icon;
            return string.Empty;
        }

        public static string? GetIconDescription(string categoryName, string iconName)
        {
            var categoryType = typeof(Core.Constants.IconConstants)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(t => t.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            if (categoryType == null) return null;
            var field = categoryType.GetField(iconName, BindingFlags.Public | BindingFlags.Static);
            if (field == null) return null;
            return $"{categoryName}.{iconName}";
        }

        public static bool IconExists(string categoryName, string iconName)
        {
            var icons = GetIconsInCategory(categoryName);
            return icons.ContainsKey(iconName);
        }

        public static string GetUnicodeHex(string iconChar)
        {
            if (string.IsNullOrEmpty(iconChar)) return string.Empty;
            int codePoint = char.ConvertToUtf32(iconChar, 0);
            return codePoint.ToString("X4");
        }

        public static string GetHtmlEntity(string iconChar)
        {
            if (string.IsNullOrEmpty(iconChar)) return string.Empty;
            string hex = GetUnicodeHex(iconChar);
            return $"&amp;#x{hex};";
        }
    }
}
