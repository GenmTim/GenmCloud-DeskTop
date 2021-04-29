using GenmCloud.Chat.Views;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Data.VO;
using GenmCloud.Views;
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

        private readonly IRegionManager regionManager;

        private MenuVO nowSelectedMenuItem;
        public MenuVO NowSelectedMenuItem 
        {
            get => nowSelectedMenuItem;
            set
            {
                nowSelectedMenuItem = value;
                RouteHelper.Route(regionManager, typeof(MainWindow), nowSelectedMenuItem.Path);
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
