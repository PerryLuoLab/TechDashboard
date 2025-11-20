using System;
using System.Globalization;
using System.Windows.Data;
using TechDashboard.Core.Constants;

namespace TechDashboard.Core.Converters;

/// <summary>
/// Converts icon name strings to their corresponding Unicode character from IconConstants
/// Usage in XAML: Text="{Binding Converter={StaticResource IconConverter}, ConverterParameter=Add}"
/// </summary>
public class IconConverter : IValueConverter
{
    /// <summary>
    /// Converts an icon name to its Unicode character
    /// </summary>
    /// <param name="value">Not used, can be null</param>
    /// <param name="targetType">Target type (should be string)</param>
    /// <param name="parameter">Icon name (e.g., "Add", "Settings", "Home")</param>
    /// <param name="culture">Culture info</param>
    /// <returns>Unicode character for the icon</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not string iconName)
            return string.Empty;

        // Try to get icon from Common category
        var commonType = typeof(IconConstants.Common);
        var field = commonType.GetField(iconName);
        if (field?.GetValue(null) is string icon)
            return icon;

        // Try to get icon from Navigation category
        var navType = typeof(IconConstants.Navigation);
        field = navType.GetField(iconName);
        if (field?.GetValue(null) is string navIcon)
            return navIcon;

        // Try to get icon from Status category
        var statusType = typeof(IconConstants.Status);
        field = statusType.GetField(iconName);
        if (field?.GetValue(null) is string statusIcon)
            return statusIcon;

        // If not found, return empty string
        return string.Empty;
    }

    /// <summary>
    /// Not implemented as this is a one-way converter
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("IconConverter is a one-way converter.");
    }
}
