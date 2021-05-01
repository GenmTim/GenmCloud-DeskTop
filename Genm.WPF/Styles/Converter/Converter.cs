using Genm.WPF.Tools.Helper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Genm.WPF.Styles.Converter
{
    class Timestamp2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            long timestamp = (long)value;
            return TimeHelper.ToDateTime(timestamp).ToString("yyyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class Timestamp2FriendlyString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            long timestamp = (long)value;
            return TimeHelper.ToDateTime(timestamp).ToString("MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
