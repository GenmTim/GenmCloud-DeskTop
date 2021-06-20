using Genm.WPF.Tools.Helper;
using GenmCloud.ApiService;
using GenmCloud.ApiService.Service;
using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Chat.Manager;
using GenmCloud.Chat.Views;
using GenmCloud.Common;
using GenmCloud.Contact.Views;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Core.Manager;
using GenmCloud.Core.Manager.Impl;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Core.UserControls.Dialog.Views;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Conf;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.DataInterfaces;
using GenmCloud.Storage.Views;
using GenmCloud.Test.Views;
using GenmCloud.Views;
using GenmCloud.Views.Login;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Configuration;
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
            // 全局单例IOC工具
            NetCoreProvider.RegisterServiceLocator(Container);

            // 注册API服务
            ConfigureServices(containerRegistry);

            // 注册对话框服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            // 注册路由辅助
            containerRegistry.RegisterSingleton<Router>(() => { return Router.GetInstance(containerRegistry); });

            // 注册本地缓存服务
            //containerRegistry.RegisterSingleton<CacheManager>(() => { return CacheManager.GetInstance(); });

            // 注册通信消息管理
            containerRegistry.RegisterSingleton<ChatMsgManager>(() => { return ChatMsgManager.GetInstance(); });

            // 注册头像管理
            containerRegistry.RegisterSingleton<UserInfoManager>(() => { return UserInfoManager.GetInstance(); });

            // 注册日志服务
            containerRegistry.RegisterSingleton<ILog, GenmCloudNLog>();

            // 注册WebSocket通信服务（用于消息通信）
            containerRegistry.RegisterSingleton<WebSocketService>();

            // 视图路由注入
            RegisterRoute();
            RegisterDialog(containerRegistry);

            // 开启本地缓存服务
            //Container.Resolve<CacheManager>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // 即时通信模块
            moduleCatalog.AddModule<Chat.ChatModule>(JsonConvert.SerializeObject(Chat.ChatModule.ModuleInfo));

            // 联系人模块
            moduleCatalog.AddModule<Contact.ContactModule>(JsonConvert.SerializeObject(Contact.ContactModule.ModuleInfo));

            // 云存储模块
            moduleCatalog.AddModule<Storage.StorageModule>(JsonConvert.SerializeObject(Storage.StorageModule.ModuleInfo));

            // 测试模块
            moduleCatalog.AddModule<Test.TestModule>(JsonConvert.SerializeObject(Test.TestModule.ModuleInfo));
        }

        protected override void OnInitialized()
        {
            InitSetting();

            var loginResult = Login();

            if (loginResult.Value)
            {
                // 初始化基本信息
                InitApplicationInfo();

                // 开启管理者和服务
                StartHandler();

                base.OnInitialized();
            }
            else
            {
                if (Application.Current != null)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void InitSetting()
        {
            Conf.ServerUrl = ConfigurationManager.AppSettings["serverAddress"];

            // 编辑器的高亮显示扩展
            TextEditorHelper.RegisterHighlighting("Go", new[] { ".go" }, "Go.xshd");
            TextEditorHelper.RegisterHighlighting("Lua", new[] { ".slua", ".lua" }, "Lua.xshd");
        }

        private bool? Login()
        {
            var login = Container.Resolve<LoginWindow>();
            RegionManager.SetRegionManager(login, Container.Resolve<IRegionManager>());
            return login.ShowDialog();
        }

        private void StartHandler()
        {
            // 头像管理者
            Container.Resolve<AvatarManager>();

            // 通信消息管理者
            Container.Resolve<ChatMsgManager>();

            // WebSocket通信服务
            Container.Resolve<WebSocketService>();
        }

        // 注册相关API服务
        private void ConfigureServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IContactService, ContactService>();
            containerRegistry.Register<IChatService, ChatService>();
            containerRegistry.Register<IUploadService, UploadService>();
            containerRegistry.Register<IFolderService, FolderService>();
            containerRegistry.Register<IDownloadService, DownloadService>();
        }

        // 注册视图路由
        private void RegisterRoute()
        {
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
                router[typeof(ContactView)] = RouteHelper.MakeRouteInfo(typeof(MainWindow), "contact/", RegionToken.MainContent);
                router[typeof(TestView)] = RouteHelper.MakeRouteInfo(typeof(MainWindow), "test/", RegionToken.MainContent);
            }

            Chat.ChatModule.ModuleInfo.Path = router[typeof(ChatView)].Path;
            Storage.StorageModule.ModuleInfo.Path = router[typeof(StorageView)].Path;
            Contact.ContactModule.ModuleInfo.Path = router[typeof(ContactView)].Path;
            Test.TestModule.ModuleInfo.Path = router[typeof(TestView)].Path;
        }

        private void RegisterDialog(IContainerRegistry containerRegistry)
        {
            // 问答对话框
            containerRegistry.RegisterForNavigation<QuestionDialog>();

            // 值输入对话框
            containerRegistry.RegisterForNavigation<InputValueDialog>();

            // 寻找联系人和群组对话框
            containerRegistry.RegisterForNavigation<QueryContactDialog>();
        }

        private async void InitApplicationInfo()
        {
            var userService = Container.Resolve<IUserService>();
            var result = await userService.GetUserInfo(0);
            if (result.StatusCode == ServiceHelper.RequestOk)
            {
                SessionService.User = result.Result;
                UserInfoManager.GetInstance().Store(SessionService.User.ID, SessionService.User);
                Container.Resolve<IEventAggregator>().GetEvent<UserInfoUpdateEvent>().Publish();
            }
        }
    }
}
