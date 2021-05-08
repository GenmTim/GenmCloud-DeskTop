using System;
using System.Windows;

namespace Genm.WPF.Attach
{
    public class UriElement
    {
        public static readonly DependencyProperty UriProperty = DependencyProperty.RegisterAttached(
"Uri", typeof(Uri), typeof(UriElement), new PropertyMetadata(default(Uri)));

        public static void SetUri(DependencyObject element, Uri value)
            => element.SetValue(UriProperty, value);

        public static Uri GetUri(DependencyObject element)
    => (Uri)element.GetValue(UriProperty);
    }
}
