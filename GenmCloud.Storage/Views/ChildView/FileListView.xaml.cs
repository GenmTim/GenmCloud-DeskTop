using GenmCloud.Shared.Common;
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

namespace GenmCloud.Storage.Views.ChildView
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
            eventAggregator.GetEvent<StorageView.ShrinkGridEvent>().Publish();
        }

        /// <summary>
        /// 展开自身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandViewEvent(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<StorageView.ExpandGridEvent>().Publish();
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

