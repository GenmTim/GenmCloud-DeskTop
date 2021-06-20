using GenmCloud.Core.Resources.Animation;
using GenmCloud.Shared.Common;
using GenmCloud.Storage.ViewModels.Component;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenmCloud.Storage.Views.Component
{
    /// <summary>
    /// DownloadPopup.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadPopup : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        public DownloadPopup()
        {
            InitializeComponent();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<DownloadPopupViewModel.DownloadHeightEvent>().Subscribe(() =>
            {
                double gridHeight = 0;
                if (DataContext is DownloadPopupViewModel vm)
                {
                    if (!vm.IsOpenPopup) return;
                    gridHeight = Math.Min(248, vm.DownloadFileList.Count * 52 + 40);
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
            if (DataContext is DownloadPopupViewModel vm)
            {
                gridHeight = Math.Min(248, vm.DownloadFileList.Count * 52 + 40);
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
