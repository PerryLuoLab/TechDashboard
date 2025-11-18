using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TechDashboard.Core.Converters
{
    /// <summary>
    /// Converts a boolean value to Visibility enumeration.
    /// True converts to Visible, False converts to Collapsed.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to Visibility.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visibility.Visible if true, Visibility.Collapsed if false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility value back to boolean.
        /// </summary>
        /// <param name="value">The Visibility value to convert.</param>
        /// <param name="targetType">The target type (bool).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>True if Visible, False otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }

            return false;
        }
    }

    /// <summary>
    /// Converts a boolean value to Visibility enumeration with inverse logic.
    /// True converts to Collapsed, False converts to Visible.
    /// </summary>
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to Visibility with inverse logic.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visibility.Collapsed if true, Visibility.Visible if false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible;
        }

        /// <summary>
        /// Converts a Visibility value back to boolean with inverse logic.
        /// </summary>
        /// <param name="value">The Visibility value to convert.</param>
        /// <param name="targetType">The target type (bool).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>True if Collapsed, False if Visible.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Collapsed;
            }

            return false;
        }
    }
}
