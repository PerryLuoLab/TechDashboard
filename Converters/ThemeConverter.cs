using System;
using System.Globalization;
using System.Windows.Data;

namespace TechDashboard.Converters
{
    /// <summary>
    /// Converts between theme names and boolean values for ToggleButton IsChecked binding.
    /// Used to synchronize theme selection buttons with the current theme.
    /// </summary>
    public class ThemeConverter : IValueConverter
    {
        /// <summary>
        /// Converts the current theme name to a boolean indicating if it matches the target theme.
        /// </summary>
        /// <param name="value">The current theme name</param>
        /// <param name="targetType">The target type (bool)</param>
        /// <param name="parameter">The target theme name to compare against</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>True if the current theme matches the target theme, false otherwise</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string currentTheme && parameter is string targetTheme)
            {
                return currentTheme.Equals(targetTheme, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Converts a boolean back to the theme name when the ToggleButton is checked.
        /// </summary>
        /// <param name="value">The IsChecked boolean value</param>
        /// <param name="targetType">The target type (string)</param>
        /// <param name="parameter">The theme name to return if checked</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>The theme name if checked, otherwise DoNothing</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string themeName)
            {
                return themeName;
            }

            return Binding.DoNothing;
        }
    }
}