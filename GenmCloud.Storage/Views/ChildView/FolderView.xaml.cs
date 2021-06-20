using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Resources.Animation;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Shared.Common;
using GenmCloud.Storage.ViewModels.ChildView;
using GenmCloud.Storage.Views.ChildView.SubItem;
using GenmCloud.Storage.Views.Component;
using Prism.Events;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GenmCloud.Storage.Views.ChildView
{
    /// <summary>
    /// FolderView.xaml 的交互逻辑
    /// </summary>
    public partial class FolderView : UserControl
    {
        public class ShrinkGridEvent : PubSubEvent { }
        public class ExpandGridEvent : PubSubEvent { }

        private readonly FolderViewModel vm;

        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        public FolderView()
        {
            InitializeComponent();
            vm = (FolderViewModel)DataContext;
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.dialogHost = NetCoreProvider.Resolve<IDialogHostService>();
            this.regionManager = NetCoreProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionToken.FileListContent, typeof(FileListView));
            Loaded += RightContentView_Loaded;
        }

        private void RightContentView_Loaded(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<ShrinkGridEvent>().Subscribe(ShrinkGrid);
            eventAggregator.GetEvent<ExpandGridEvent>().Subscribe(ExpandGrid);
        }

        #region 视图收缩

        /// <summary>
        /// 左视图收缩
        /// </summary>
        private void ShrinkGrid()
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(0, GridUnitType.Pixel),
                To = new GridLength(220, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(1.5),
                EasingFunction = new PowerEase() { Power = 20, EasingMode = EasingMode.EaseOut }
            };
            mainGrid.ColumnDefinitions[1].BeginAnimation(ColumnDefinition.WidthProperty, d);
        }

        /// <summary>
        /// 左视图伸展
        /// </summary>
        private void ExpandGrid()
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(220, GridUnitType.Pixel),
                To = new GridLength(0, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(1.5),
                EasingFunction = new PowerEase() { Power = 20, EasingMode = EasingMode.EaseOut }
            };
            mainGrid.ColumnDefinitions[1].BeginAnimation(ColumnDefinition.WidthProperty, d);
        }
        #endregion
    }
}
