using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Helpers
{
    public class AllValuesBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {

                if (value == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}