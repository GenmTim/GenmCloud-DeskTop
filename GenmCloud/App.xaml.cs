using Genm.WPF.Tools.Helper;
using GenmCloud.Chat.Views;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Manager;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Storage.Views;
using GenmCloud.Views;
using GenmCloud.Views.Login;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.IO;
using System.Windows;
using System.Xml;
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
            containerRegistry.RegisterSingleton<ChatMsgManager>();
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
                router[typeof(StorageView)] = RouteHelper.MakeRouteInfo(typeof(MainWindow), "storage/", RegionToken.MainContent);
            }

            Chat.ChatModule.ModuleInfo.Path = router[typeof(ChatView)].Path;
            Storage.StorageModule.ModuleInfo.Path = router[typeof(StorageView)].Path;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Chat.ChatModule>(JsonConvert.SerializeObject(Chat.ChatModule.ModuleInfo));
            moduleCatalog.AddModule<Storage.StorageModule>(JsonConvert.SerializeObject(Storage.StorageModule.ModuleInfo));
        }

        protected override void OnInitialized()
        {
            var login = Container.Resolve<LoginWindow>();
            RegionManager.SetRegionManager(login, Container.Resolve<IRegionManager>());

            var result = login.ShowDialog();
            if (result.Value)
            {
                // 开启各个管理者和服务
                Container.Resolve<ChatMsgManager>();

                // 编辑器的高亮显示扩展
                TextEditorHelper.RegisterHighlighting("Go", new[] { ".go" }, "Go.xshd");
                TextEditorHelper.RegisterHighlighting("Lua", new[] { ".slua", ".lua" }, "Lua.xshd");
                
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
