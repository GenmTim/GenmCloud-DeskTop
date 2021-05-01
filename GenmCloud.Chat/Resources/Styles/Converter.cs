using GenmCloud.Core.Data.Type;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GenmCloud.Chat.Resources.Styles
{
    class JudgeIdToRole : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                throw new Exception("值是否是错误的");
            }
            long id = (long)value;
            return id == 0 ? ChatRoleType.Me : ChatRoleType.Other;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
