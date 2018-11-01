using System;
using System.Globalization;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    internal class BoolToIntConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a bool to 0 or 1
        /// </summary>
        /// <param name="value">The bool to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture</param>
        /// <returns>1 if true. Otherwise 0</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool) value ? 1 : 0;
        }

        /// <summary>
        ///     Converts int to bool
        /// </summary>
        /// <param name="value">The int to convert</param>
        /// <param name="targetType">Not Used</param>
        /// <param name="parameter">Not Used</param>
        /// <param name="culture">Not Used</param>
        /// <returns>True if 1. Otherwise false</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (int) value == 1 ? true : (object) false;
        }
    }
}