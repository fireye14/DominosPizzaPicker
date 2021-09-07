using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                if (!(value is string)) return value;
                var input = value.ToString();
                var numDec = System.Convert.ToInt16(parameter);

                // If there are more than numDec digits after the decimal, take off the last digit and return, otherwise return input value
                if (input.Contains(".") && input.Substring(input.IndexOf(".") + 1).Length > numDec)
                    return input.Substring(0, input.IndexOf(".") + 1) + input.Substring(input.IndexOf(".") + 1, numDec);

                return value;

            }
            catch { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
