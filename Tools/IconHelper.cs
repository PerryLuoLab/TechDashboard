using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TechDashboard.Tools
{
    /// <summary>
    /// Helper class for working with icon constants. 
    /// Includes caching to optimize performance for Converters and Pickers.
    /// </summary>
    public static class IconHelper
    {
        // Cache for all icons: "Category.Name" -> "Value"
        private static readonly Lazy<Dictionary<string, string>> _allIconsCache = new Lazy<Dictionary<string, string>>(LoadAllIcons);

        // Cache for simple name lookup: "Name" -> "Value" (Respecting precedence: Common > Navigation > Status)
        private static readonly Lazy<Dictionary<string, string>> _simpleLookupCache = new Lazy<Dictionary<string, string>>(LoadSimpleLookup);

        public static IEnumerable<string> GetCategories()
        {
            return typeof(Core.Constants.IconConstants)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .Select(t => t.Name);
        }

        public static Dictionary<string, string> GetIconsInCategory(string categoryName)
        {
            // We can still use reflection here for specific category requests (used by the loaders)
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

        public static Dictionary<string, string> GetAllIcons() => _allIconsCache.Value;

        private static Dictionary<string, string> LoadAllIcons()
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

        private static Dictionary<string, string> LoadSimpleLookup()
        {
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);

            // Precedence: Common (highest) > Navigation > Status (lowest)
            // We iterate in order and only add if not present (First wins)
            var categories = new[] { "Common", "Navigation", "Status" };

            foreach (var cat in categories)
            {
                var icons = GetIconsInCategory(cat);
                foreach (var kvp in icons)
                {
                    if (!dict.ContainsKey(kvp.Key))
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
            }
            return dict;
        }

        public static Dictionary<string, string> SearchIcons(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new Dictionary<string, string>();

            return GetAllIcons()
                .Where(kvp => kvp.Key.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static string GetIcon(string iconName)
        {
            if (string.IsNullOrWhiteSpace(iconName))
                return string.Empty;

            // Use cached lookup O(1) instead of Reflection
            if (_simpleLookupCache.Value.TryGetValue(iconName, out var val))
                return val;

            return string.Empty;
        }

        public static string? GetIconDescription(string categoryName, string iconName)
        {
            // This is rarely called, reflection is fine or we could cache if needed
            if (IconExists(categoryName, iconName))
                return $"{categoryName}.{iconName}";
            return null;
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
            return $"&#x{hex};";
        }
    }
}