using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Shared.Dto;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Views.Login
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class SignInView : UserControl
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public SignInView(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<SignInEvent>().Publish(new LoginDto { Username = usernameBox.Text, Password = passwordBox.Password });
            e.Handled = true;
        }

        private void JumpToRegisterPage(object sender, RoutedEventArgs e)
        {
            RegionHelper.RequestNavigate(regionManager, RegionToken.LoginContent, RouteHelper.GetPath(typeof(SignUpView)));
        }
    }
}
