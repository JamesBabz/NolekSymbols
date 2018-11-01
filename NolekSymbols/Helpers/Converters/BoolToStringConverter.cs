using System;
using System.Globalization;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    internal class BoolToStringConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a bool to a string given as parameter
        /// </summary>
        /// <param name="value">´The bool</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">The string to return. Format "True:False"</param>
        /// <param name="culture">Culture</param>
        /// <returns>the value if its not null. An empty string if the value was null</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return null;
            var stringArray = parameter.ToString().Split(':');
            return value != null && (bool) value ? stringArray[0] : stringArray[1];
        }

        /// <summary>
        ///     Not Implemented
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