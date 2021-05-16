using Genm.WPF.Data.VO;
using GenmCloud.Core.Data;
using GenmCloud.Core.Event;
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

        private readonly IRegionManager regionManager;
        private readonly IModuleCatalog moduleCatalog;
        private readonly IEventAggregator eventAggregator;

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

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IModuleCatalog moduleCatalog)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.moduleCatalog = moduleCatalog;
            this.eventAggregator.GetEvent<ShowNameCardEvent>().Subscribe(ShowNameCard);
            InitRouteMenuList();
        }

        private void ShowNameCard()
        {
            IsNameCardOpen = true;
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
