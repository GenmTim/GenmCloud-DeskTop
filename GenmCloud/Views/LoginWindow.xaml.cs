using Genm.WPF.Controls;
using Genm.WPF.Data.Interface;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Views.Login;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : BaseDialogWindow, IRouteRegion
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public LoginWindow(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Loaded += (o, s) => { RegionHelper.RequestNavigate(regionManager, RegionToken.LoginContent, typeof(SignInView)); };
            eventAggregator.GetEvent<SignedInEvent>().Subscribe(SignedIn);
        }

        public static string GetRouteRegionName()
        {
            return RegionToken.LoginContent;
        }

        private void SignedIn()
        {
            DialogResult = true;
        }
    }
}
