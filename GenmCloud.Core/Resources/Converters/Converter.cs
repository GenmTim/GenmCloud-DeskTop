using Genm.WPF.Tools.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenmCloud.Core.Resources.Converters
{
    public class LinearGradientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            Color color = (Color)ColorConverter.ConvertFromString((string)value); ;
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(8, 0.5)
            };
            linearGradientBrush.GradientStops.Add(new GradientStop(color, 0));
            linearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 1));
            return linearGradientBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class FetchFirstCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            String str = (String)value;
            return str.Length > 0 ? str.Substring(0, 1) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class String2UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            String str = (String)value;
            return new Uri(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = (Uri)value;
            return uri.ToString();
        }
    }

    public class String2FileType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            String aFile = (String)value;
            string aLastName = aFile.Substring(aFile.LastIndexOf(".") + 1, (aFile.Length - aFile.LastIndexOf(".") - 1));
            return aLastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class FileSize2String : IValueConverter
    {
        private const decimal OneKiloByte = 1024M;
        private const decimal OneMegaByte = OneKiloByte * 1024M;
        private const decimal OneGigaByte = OneMegaByte * 1024M;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            long fileSize = (long)value;
            string suffix;

            decimal size = System.Convert.ToDecimal(fileSize);
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "KB";
            }
            else
            {
                suffix = "B";
            }
            return size.ToString("#0.00") + " " + suffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class String2FileTypeIconPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            String aFile = (String)value;
            string fileTypeName = aFile.Substring(aFile.LastIndexOf(".") + 1, (aFile.Length - aFile.LastIndexOf(".") - 1));
            string basePath = "http://localhost:1026/static/Icon/FileType/";
            string path = basePath;

            switch (fileTypeName)
            {
                case "ppt":
                case "PPT":
                case "pptx":
                    path += "PPT.png";
                    break;
                case "exe":
                    path += "Exe.png";
                    break;
                case "doc":
                case "docx":
                    path += "Word.png";
                    break;
                case "pdf":
                    path += "PDF.png";
                    break;
                case "xlg":
                    path += "Excel.png";
                    break;
                case "jpg":
                case "png":
                    path += "Img.png";
                    break;
                default:
                    path += "Other.png";
                    break;
            }
            return new BitmapImage(new Uri(path));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class String2FileName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            String aFile = (String)value;
            string aFirstName = aFile.Substring(aFile.LastIndexOf("\\") + 1, (aFile.LastIndexOf(".") - aFile.LastIndexOf("\\") - 1));
            return aFirstName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Timestamp2String : IValueConverter
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

    public class TreeViewItemMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var left = 0.0;
            UIElement element = value as TreeViewItem;
            while (element != null && element.GetType() != typeof(TreeView))
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                if (element is TreeViewItem)
                    left += 14.0;
            }
            return new Thickness(left, 0, 0, 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class UriToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = (Uri)value;
            BitmapImage bmp = new BitmapImage();
            bmp.DecodePixelHeight = 250; // 确定解码高度，宽度不同时设置
            bmp.BeginInit();
            // 延迟，必要时创建
            bmp.CreateOptions = BitmapCreateOptions.DelayCreation;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = uri;
            bmp.EndInit(); //结束初始化
            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public sealed class StringToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uriStr = (string)value;
            if (uriStr == null) return null;

            Uri uri = new Uri(uriStr);
            BitmapImage bmp = new BitmapImage();
            bmp.DecodePixelHeight = 250; // 确定解码高度，宽度不同时设置
            bmp.BeginInit();
            // 延迟，必要时创建
            bmp.CreateOptions = BitmapCreateOptions.DelayCreation;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = uri;
            bmp.EndInit(); //结束初始化
            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    //public class String2UriConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is null) return "";
    //        string str = (string)value;

    //        return new Uri(str);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Number2PercentageConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (values == null || values.Length != 2) return .0;

    //        var obj1 = values[0];
    //        var obj2 = values[1];

    //        if (obj1 == null || obj2 == null) return .0;

    //        var str1 = values[0].ToString();
    //        var str2 = values[1].ToString();

    //        var v1 = double.Parse(str1);
    //        var v2 = double.Parse(str2);

    //        if (Math.Abs(v2) < 1E-06) return 100.0;

    //        return Math.Round(v1 / v2 * 100, 0);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
