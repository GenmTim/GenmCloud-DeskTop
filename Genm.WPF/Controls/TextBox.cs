using System.Windows.Input;

namespace Genm.WPF.Controls
{
    public class TextBox : System.Windows.Controls.TextBox
    {
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
        }
    }
}
