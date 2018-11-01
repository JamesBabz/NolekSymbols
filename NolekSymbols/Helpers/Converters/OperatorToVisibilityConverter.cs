using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    public class OperatorToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Sets visibility of object based on the operators value
        /// </summary>
        /// <param name="value">The value of the operator</param>
        /// <param name="targetType"> Target Type</param>
        /// <param name="parameter">Is the text field on the left side or the right side of the operator</param>
        /// <param name="culture">Culture</param>
        /// <returns>Visibility visible or collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string paramString)) return false;
            if (paramString.Equals("left"))
            {
                if (!(value is string stringValue) || stringValue.Equals("Default"))
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
            if (!paramString.Equals("right")) return false;
            {
                if (!(value is string stringValue) || stringValue.Equals("-"))
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Not implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}