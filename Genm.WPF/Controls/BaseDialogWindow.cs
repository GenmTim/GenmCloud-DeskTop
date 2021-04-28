using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Genm.WPF.Controls
{
    public class BaseDialogWindow
        : Window
    {
        protected void CloseDialog(object sender, RoutedEventArgs e)
        {
            //Question?
            Application.Current.Shutdown();
        }

        protected void MinimizedDialog(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        protected void MaximizedDialog(object sender, RoutedEventArgs e)
        {
            this.SetWindowSize();
        }

        protected void SetWindowSize()
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }
    }
}
