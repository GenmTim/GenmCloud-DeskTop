using GenmCloud.Core.Event;
using GenmCloud.Shared.Dto;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.Views.Login
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class SignInView : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        public SignInView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<SignInEvent>().Publish(new LoginDto { Account = usernameBox.Text, PassWord = passwordBox.Password });
            e.Handled = true;
        }
    }
}
