using Genm.WPF.Tools.Helper;
using GenmCloud.Chat.Views;
using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Core.Data.Token;
using GenmCloud.Views;
using GenmCloud.Views.Login;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<Router>(() => 
            {
                return Router.Instance(containerRegistry);
            });
            var router = Container.Resolve<Router>();

            router[typeof(LoginWindow)] = RouteHelper.MakeRouteInfo("login/");
            {
                router[typeof(SignInView)] = RouteHelper.MakeRouteInfo(typeof(LoginWindow), "signin/", RegionToken.LoginContent);
                router[typeof(SignUpView)] = RouteHelper.MakeRouteInfo(typeof(LoginWindow), "signup/", RegionToken.LoginContent);
            }

            router[typeof(MainWindow)] = new RouteInfo { Path = "main/" };
            {
                router[typeof(ChatView)] = RouteHelper.MakeRouteInfo(typeof(MainWindow), "chat/", RegionToken.MainContent);
            }
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Chat.ChatModule>();
        }

        protected override void OnInitialized()
        {
            var login = Container.Resolve<LoginWindow>();
            RegionManager.SetRegionManager(login, Container.Resolve<IRegionManager>());

            var result = login.ShowDialog();
            if (result.Value)
            {
                base.OnInitialized();
            }
            else
            {
                if (Application.Current != null)
                    Application.Current.Shutdown();
            }
        }
    }
}
