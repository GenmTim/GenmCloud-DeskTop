using GenmCloud.Core.UserControls.Common.Views;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.DataInterfaces;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace UsageTestProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 全局单例IOC工具
            NetCoreProvider.RegisterServiceLocator(Container);

            Contract.ServerUrl = "http://localhost:1026/api/v1/";

            containerRegistry.Register<UploadInfoPopup>();

            SessionService.Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6MywiZXhwIjoxNjI0MTY4ODI4LCJpYXQiOjE2MjM1NjQwMjgsImlzcyI6Imdlbm0iLCJzdWIiOiJ1c2VyIHRva2VuIn0.o5D-1xZDLySAQ7js0BNDPQMwI8UdWiGAoRczHttkJGk";
        }
    }
}
