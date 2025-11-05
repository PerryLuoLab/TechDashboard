using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TechDashboard.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string currentLang && parameter is string targetLang)
            {
                return currentLang.Equals(targetLang, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

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