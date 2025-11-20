using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TechDashboard.Core.Constants;

/// <summary>
/// Helper class for working with icon constants
/// Provides utility methods for discovering and using icons
/// </summary>
public static class IconHelper
{
    /// <summary>
    /// Gets all available icon categories
    /// </summary>
    /// <returns>List of category names</returns>
    public static IEnumerable<string> GetCategories()
    {
        return typeof(IconConstants)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
            .Select(t => t.Name);
    }

    /// <summary>
    /// Gets all icons in a specific category
    /// </summary>
    /// <param name="categoryName">Category name (e.g., "Common", "Navigation", "Status")</param>
    /// <returns>Dictionary of icon name to Unicode character</returns>
    public static Dictionary<string, string> GetIconsInCategory(string categoryName)
    {
        var categoryType = typeof(IconConstants)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(t => t.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        if (categoryType == null)
            return new Dictionary<string, string>();

        return categoryType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .ToDictionary(f => f.Name, f => f.GetValue(null)?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Gets all icons from all categories
    /// </summary>
    /// <returns>Dictionary of icon full name to Unicode character</returns>
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

    /// <summary>
    /// Searches for icons by name (case-insensitive)
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>Dictionary of matching icon names to Unicode characters</returns>
    public static Dictionary<string, string> SearchIcons(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new Dictionary<string, string>();

        var allIcons = GetAllIcons();
        return allIcons
            .Where(kvp => kvp.Key.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Gets icon by name from any category
    /// </summary>
    /// <param name="iconName">Icon name (e.g., "Add", "Settings")</param>
    /// <returns>Unicode character if found, empty string otherwise</returns>
    public static string GetIcon(string iconName)
    {
        if (string.IsNullOrWhiteSpace(iconName))
            return string.Empty;

        // Try Common first (most likely)
        var commonIcons = GetIconsInCategory("Common");
        if (commonIcons.TryGetValue(iconName, out var icon))
            return icon;

        // Try Navigation
        var navIcons = GetIconsInCategory("Navigation");
        if (navIcons.TryGetValue(iconName, out icon))
            return icon;

        // Try Status
        var statusIcons = GetIconsInCategory("Status");
        if (statusIcons.TryGetValue(iconName, out icon))
            return icon;

        return string.Empty;
    }

    /// <summary>
    /// Gets icon documentation from XML comments
    /// </summary>
    /// <param name="categoryName">Category name</param>
    /// <param name="iconName">Icon name</param>
    /// <returns>Icon description or null if not found</returns>
    public static string? GetIconDescription(string categoryName, string iconName)
    {
        var categoryType = typeof(IconConstants)
            .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(t => t.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        if (categoryType == null)
            return null;

        var field = categoryType.GetField(iconName, BindingFlags.Public | BindingFlags.Static);
        if (field == null)
            return null;

        // Note: Getting XML documentation requires additional reflection with XML documentation file
        // For now, return field name as description
        return $"{categoryName}.{iconName}";
    }

    /// <summary>
    /// Validates if an icon exists
    /// </summary>
    /// <param name="categoryName">Category name</param>
    /// <param name="iconName">Icon name</param>
    /// <returns>True if icon exists, false otherwise</returns>
    public static bool IconExists(string categoryName, string iconName)
    {
        var icons = GetIconsInCategory(categoryName);
        return icons.ContainsKey(iconName);
    }

    /// <summary>
    /// Gets Unicode hex code for an icon
    /// </summary>
    /// <param name="iconChar">Icon Unicode character</param>
    /// <returns>Hex code (e.g., "E710")</returns>
    public static string GetUnicodeHex(string iconChar)
    {
        if (string.IsNullOrEmpty(iconChar))
            return string.Empty;

        int codePoint = char.ConvertToUtf32(iconChar, 0);
        return codePoint.ToString("X4");
    }

    /// <summary>
    /// Gets HTML entity for an icon
    /// </summary>
    /// <param name="iconChar">Icon Unicode character</param>
    /// <returns>HTML entity (e.g., "&amp;#xE710;")</returns>
    public static string GetHtmlEntity(string iconChar)
    {
        if (string.IsNullOrEmpty(iconChar))
            return string.Empty;

        string hex = GetUnicodeHex(iconChar);
        return $"&amp;#x{hex};";
    }

    /// <summary>
    /// Prints all icons to console (useful for debugging)
    /// </summary>
    public static void PrintAllIcons()
    {
        Console.WriteLine("=== Available Icons ===\n");

        foreach (var category in GetCategories())
        {
            Console.WriteLine($"Category: {category}");
            Console.WriteLine(new string('-', 50));

            var icons = GetIconsInCategory(category);
            foreach (var icon in icons.OrderBy(i => i.Key))
            {
                string hex = GetUnicodeHex(icon.Value);
                Console.WriteLine($"  {icon.Key,-30} {icon.Value}  (U+{hex})");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Gets icon usage example in XAML
    /// </summary>
    /// <param name="categoryName">Category name</param>
    /// <param name="iconName">Icon name</param>
    /// <returns>XAML code example</returns>
    public static string GetXamlExample(string categoryName, string iconName)
    {
        return $@"<TextBlock Text=""{{x:Static icons:IconConstants+{categoryName}.{iconName}}}"" 
           FontFamily=""{{x:Static icons:IconConstants.DefaultFontFamily}}"" 
           FontSize=""16""/>";
    }

    /// <summary>
    /// Gets icon usage example in C#
    /// </summary>
    /// <param name="categoryName">Category name</param>
    /// <param name="iconName">Icon name</param>
    /// <returns>C# code example</returns>
    public static string GetCSharpExample(string categoryName, string iconName)
    {
        return $"string icon = IconConstants.{categoryName}.{iconName};";
    }
}
