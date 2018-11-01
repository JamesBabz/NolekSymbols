using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    public class BooltoVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a bool Visibility.visible or Visibility.collapsed
        /// </summary>
        /// <param name="value">The null string</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture</param>
        /// <returns>Visibility.visible if 1. Otherwise Visibility.collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString().Equals("inverse"))
            {
                if (value is bool b && b)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
            else
            {
                if (value is bool b && b)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Converts Visibility to bool
        /// </summary>
        /// <param name="value">The Visibility to convert</param>
        /// <param name="targetType">Not Used</param>
        /// <param name="parameter">Not Used</param>
        /// <param name="culture">Not Used</param>
        /// <returns>True if Visibility.visible. Otherwise false</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}