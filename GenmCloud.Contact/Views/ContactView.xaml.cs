using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.UserControls.Dialog.Views;
using Prism.Events;
using System.Windows.Controls;

namespace GenmCloud.Contact.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ContactView : UserControl
    {
        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;

        public ContactView(IDialogHostService dialogHost, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.dialogHost = dialogHost;
            this.eventAggregator = eventAggregator;
            Loaded += (s, o) =>
            {
                this.eventAggregator.GetEvent<ContactListUpdateEvent>().Publish();
            };
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if ((menuItem.Tag != null) && menuItem.Tag.Equals("AddNewContact"))
                {
                    dialogHost.ShowDialog(nameof(QueryContactDialog), null, "ContactDialogRoot");
                }
            }
            menuPopup.IsOpen = false;
            e.Handled = true;
        }
    }
}
