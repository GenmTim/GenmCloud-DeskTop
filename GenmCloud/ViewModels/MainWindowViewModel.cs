using Genm.WPF.Data.VO;
using GenmCloud.ApiService.Service;
using GenmCloud.Chat.Views;
using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Core.Data;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Views;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.ViewModels
{


    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public List<RouteMenuVO> menuList;
        public List<RouteMenuVO> MenuList
        {
            get => menuList;
            set
            {
                menuList = value;
                RaisePropertyChanged();
            }
        }

        private string avatar;
        public string Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                RaisePropertyChanged();
            }
        }

        private readonly IRegionManager regionManager;
        private readonly IModuleCatalog moduleCatalog;
        private readonly IEventAggregator eventAggregator;

        private readonly IUserService userService;

        private RouteMenuVO nowSelectedMenuItem;
        public RouteMenuVO NowSelectedMenuItem
        {
            get => nowSelectedMenuItem;
            set
            {
                nowSelectedMenuItem = value;
                RouteHelper.Route(regionManager, typeof(MainWindow), nowSelectedMenuItem.Path);
                RaisePropertyChanged();
            }
        }

        private bool isNameCardOpen;
        public bool IsNameCardOpen
        {
            get => isNameCardOpen;
            set
            {
                isNameCardOpen = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.regionManager = NetCoreProvider.Resolve<IRegionManager>();
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.moduleCatalog = NetCoreProvider.Resolve<IModuleCatalog>();
            this.userService = NetCoreProvider.Resolve<IUserService>();
            this.eventAggregator.GetEvent<UserInfoUpdateEvent>().Subscribe(() =>
            {
                Avatar = SessionService.User.Avatar;
            });
            this.eventAggregator.GetEvent<GoChatEvent>().Subscribe((id) =>
            {
                NowSelectedMenuItem = MenuList[0];
                var param = new NavigationParameters
                {
                    { "chatObjId", id }
                };
                RegionHelper.RequestNavigate(regionManager, RegionToken.MainContent, typeof(ChatView), param);
            });
            InitRouteMenuList();
        }

        private void InitRouteMenuList()
        {
            var modules = moduleCatalog.Modules;
            var menuList = new List<RouteMenuVO>();
            foreach (var item in modules)
            {
                var itr = JsonConvert.DeserializeObject<GModuleInfo>(item.ModuleName);
                menuList.Add(new RouteMenuVO { Geometry = itr.Geometry, Tip = new TextBlock { Text = itr.Name, Margin = new Thickness(3) }, UnreadMsgNumber = 0, ShowFix = itr.ShowFix, Path = itr.Path });
            }
            MenuList = menuList;
        }
    }
}
