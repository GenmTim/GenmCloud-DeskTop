using GenmCloud.Shared.Common;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using static GenmCloud.Storage.Views.ChildView.FolderView;

namespace GenmCloud.Storage.Views.ChildView.SubItem
{
    /// <summary>
    /// FileListView.xaml 的交互逻辑
    /// </summary>
    public partial class FileListView : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        public FileListView()
        {
            InitializeComponent();
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
        }

        private void CheckToGridView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("WrapModelViewer") as Style;
        }

        private void CheckToListView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("GridViewModelViewer") as Style;
            UpdateGrid();
        }

        /// <summary>
        /// 收缩自身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShrinkViewEvent(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<ShrinkGridEvent>().Publish();
        }

        /// <summary>
        /// 展开自身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandViewEvent(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<ExpandGridEvent>().Publish();
        }

        private void SimplePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (listView.View is GridView gridView)
            {
                for (int i = 0; i < Math.Min(gridView.Columns.Count, referenceGrid.ColumnDefinitions.Count); i++)
                {
                    gridView.Columns[i].Width = referenceGrid.ColumnDefinitions[i].ActualWidth;
                }
            }
        }
    }
}

