using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Dto;
using Prism.Events;
using System.Windows.Controls;

namespace GenmCloud.Views.Login
{
    /// <summary>
    /// RegisterView.xaml 的交互逻辑
    /// </summary>
    public partial class SignUpView : UserControl
    {
        private readonly IEventAggregator eventAggregator;
        public SignUpView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
        }

        private void RegisterBtnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            eventAggregator.GetEvent<SignUpEvent>().Publish(new LoginDto { Username = usernameBox.Text, Password = ServiceHelper.SetPassword(passwordBox.Password) });
            e.Handled = true;
        }
    }
}
