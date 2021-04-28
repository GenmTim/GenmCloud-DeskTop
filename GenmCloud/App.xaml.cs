using Genm.WPF.Tools.Helper;
using GenmCloud.Views;
using GenmCloud.Views.Login;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows;

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
            var routeMap = Router.Instance.RouteMap;
            
            routeMap[typeof(LoginWindow)] = "login/";
            {
                routeMap[typeof(SignInView)] = routeMap[typeof(LoginWindow)] + "signin/";
                routeMap[typeof(SignUpView)] = routeMap[typeof(LoginWindow)] + "signup/";
            }

            routeMap[typeof(MainWindow)] = "main/";

            // 进行注册
            foreach (KeyValuePair<Type, string> kv in routeMap)
            {
                containerRegistry.RegisterForNavigation(kv.Key, kv.Value);
            }
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<Chat.ChatModule>();
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
