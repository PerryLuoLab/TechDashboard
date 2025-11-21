using System;
using System.Globalization;
using System.Windows.Data;
using TechDashboard.Tools;

namespace TechDashboard.Core.Converters;

/// <summary>
/// Converts icon name strings to their corresponding Unicode character from IconConstants
/// Usage in XAML: Text="{Binding Converter={StaticResource IconConverter}, ConverterParameter=Add}"
/// </summary>
public class IconConverter : IValueConverter
{
    /// <summary>
    /// Converts an icon name to its Unicode character using cached lookup
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not string iconName)
            return string.Empty;

        // Use cached helper instead of reflection for performance
        return IconHelper.GetIcon(iconName);
    }

    /// <summary>
    /// Not implemented as this is a one-way converter
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("IconConverter is a one-way converter.");
    }
}