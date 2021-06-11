using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Genm.WPF.Attach
{
    public class ForegroundSwitchElement
    {
        public static readonly DependencyProperty MouseHoverBackgroundProperty = DependencyProperty.RegisterAttached(
"MouseHoverForeground", typeof(Brush), typeof(ForegroundSwitchElement), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached(
"MouseDownBackground", typeof(Brush), typeof(ForegroundSwitchElement), new PropertyMetadata(default(Brush)));

        public static Brush GetMouseDownForeground(DependencyObject element) => (Brush)element.GetValue(MouseDownBackgroundProperty);
        public static Brush GetMouseHoverForeground(DependencyObject element) => (Brush)element.GetValue(MouseHoverBackgroundProperty);

        public static void SetMouseHoverForeground(DependencyObject element, Brush value) => element.SetValue(MouseHoverBackgroundProperty, value);
        public static void SetMouseDownForeground(DependencyObject element, Brush value) => element.SetValue(MouseDownBackgroundProperty, value);
    }
}
