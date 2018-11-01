using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace NolekSymbols.Helpers.Converters
{
    public class ByteArrayToHexStringConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a byte array to a string
        /// </summary>
        /// <param name="value">The byte array</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Culture</param>
        /// <returns>The value if its not null. An empty string if the value was null</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ba = (byte[]) value;
            if (ba == null) return null;
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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