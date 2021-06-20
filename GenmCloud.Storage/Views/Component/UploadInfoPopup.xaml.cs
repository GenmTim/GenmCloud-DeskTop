using GenmCloud.Core.Resources.Animation;
using GenmCloud.Shared.Common;
using GenmCloud.Storage.ViewModels.Component;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GenmCloud.Storage.Views.Component
{
    /// <summary>
    /// UploadInfoPopup.xaml 的交互逻辑
    /// </summary>
    public partial class UploadInfoPopup : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        public UploadInfoPopup()
        {
            InitializeComponent();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<UploadInfoPopupViewModel.UploadHeightEvent>().Subscribe(() =>
            {
                double gridHeight = 0;
                if (DataContext is UploadInfoPopupViewModel vm)
                {
                    if (!vm.IsOpenPopup) return;
                    gridHeight = Math.Min(248, vm.UploadFileList.Count * 52 + 40);
                }
                GridLengthAnimation d = new GridLengthAnimation
                {
                    From = new GridLength(grid.RowDefinitions[1].ActualHeight, GridUnitType.Pixel),
                    To = new GridLength(gridHeight, GridUnitType.Pixel),
                    Duration = TimeSpan.FromSeconds(0.15)
                };
                grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);
            });
            listsControl.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            scrollViewer?.ScrollToBottom();
        }

        private void OpenPopup(object sender, System.Windows.RoutedEventArgs e)
        {
            double gridHeight = 0;
            if (DataContext is UploadInfoPopupViewModel vm)
            {
                gridHeight = Math.Min(248, vm.UploadFileList.Count * 52 + 40);
            }
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(0, GridUnitType.Pixel),
                To = new GridLength(gridHeight, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(0.15)
            };
            grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);

        }

        private void ShrinkPopop(object sender, RoutedEventArgs e)
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(grid.RowDefinitions[1].ActualHeight, GridUnitType.Pixel),
                To = new GridLength(0, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(0.15),
            };
            grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);
        }
    }
}
