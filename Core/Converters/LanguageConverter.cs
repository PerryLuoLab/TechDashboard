using System;
using System.Globalization;
using System.Windows.Data;

namespace TechDashboard.Core.Converters
{
    /// <summary>
    /// Converts between language codes and boolean values for ToggleButton IsChecked binding.
    /// Used to synchronize language selection buttons with the current language.
    /// </summary>
    public class LanguageConverter : IValueConverter
    {
        /// <summary>
        /// Converts the current language code to a boolean indicating if it matches the target language.
        /// </summary>
        /// <param name="value">The current language code (e.g., "en-US", "zh-CN").</param>
        /// <param name="targetType">The target type (bool).</param>
        /// <param name="parameter">The target language code to compare against.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>True if the current language matches the target language, false otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string currentLang && parameter is string targetLang)
            {
                return currentLang.Equals(targetLang, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Converts a boolean back to the language code when the ToggleButton is checked.
        /// </summary>
        /// <param name="value">The IsChecked boolean value.</param>
        /// <param name="targetType">The target type (string).</param>
        /// <param name="parameter">The language code to return if checked.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The language code if checked, otherwise Binding.DoNothing.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string languageCode)
            {
                return languageCode;
            }
            return Binding.DoNothing;
        }
    }
}
