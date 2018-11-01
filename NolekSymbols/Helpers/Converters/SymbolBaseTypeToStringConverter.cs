using System;
using System.Globalization;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    public class SymbolBaseTypeToStringConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a null string to an empty string
        /// </summary>
        /// <param name="value">The null string</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">Culture</param>
        /// <returns>the value if its not null. An empty string if the value was null</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var memberInfo = value.GetType().BaseType;
            return memberInfo == null
                ? ""
                : memberInfo.Name.Equals("SymbolModel")
                    ? value.GetType().Name
                    : memberInfo.Name;
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