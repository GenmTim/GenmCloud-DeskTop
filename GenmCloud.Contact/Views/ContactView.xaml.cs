using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenmCloud.Contact.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ContactView : UserControl
    {
        private readonly IDialogHostService dialogHost;

        public ContactView(IDialogHostService dialogHost)
        {
            InitializeComponent();
            this.dialogHost = dialogHost;
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if ((menuItem.Tag != null) && menuItem.Tag.Equals("AddNewContact"))
                {
                    DialogHelper.ShowInputValueDialog(dialogHost, "ContactDialogRoot", "申请联系人", "邀请", "取消", "请输入联系人的邮箱");
                }
            }
            menuPopup.IsOpen = false;
            e.Handled = true;
        }
    }
}
