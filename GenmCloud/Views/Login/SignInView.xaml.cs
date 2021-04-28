using GenmCloud.Core.Event;
using GenmCloud.Shared.Dto;
using Prism.Events;
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
