using Genm.WPF.Controls;
using Genm.WPF.Data.Event;
using Genm.WPF.Data.Interface;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Views.Login;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : BaseDialogWindow, IRouteView
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public LoginWindow(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Loaded += RegisterDefaultRegionView;
            eventAggregator.GetEvent<SignedInEvent>().Subscribe(SignedIn);
            eventAggregator.GetEvent<ToastShowEvent>().Subscribe(ToastShow);
            eventAggregator.GetEvent<RunGlobalProgressEvent>().Subscribe(GlobalProgress);
        }

        public void RegisterDefaultRegionView(object sender, RoutedEventArgs e)
        {
            RegionHelper.RequestNavigate(regionManager, RegionToken.LoginContent, typeof(SignInView));
        }

        private void SignedIn()
        {
            DialogResult = true;
        }

        private void ToastShow(string message)
        {
            toast.Show(message);
        }

        private void GlobalProgress(bool isRun)
        {
            if (isRun)
            {
                globalProgress.IsIndeterminate = true;
                globalProgress.Visibility = Visibility.Visible;
            }
            else
            {
                globalProgress.IsIndeterminate = false;
                globalProgress.Visibility = Visibility.Collapsed;
            }
        }
    }
}
