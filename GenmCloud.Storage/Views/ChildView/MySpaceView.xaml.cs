using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Core.UserControls.Common.Views;
using GenmCloud.Shared.Common;
using GenmCloud.Storage.ViewModels.ChildView;
using GenmCloud.Storage.Views.ChildView.SubItem;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenmCloud.Storage.Views.ChildView
{
    /// <summary>
    /// RightContentView.xaml 的交互逻辑
    /// </summary>
    public partial class MySpaceView : UserControl
    {
        public class ShrinkGridEvent : PubSubEvent { }
        public class ExpandGridEvent : PubSubEvent { }

        private readonly MySpaceViewModel  vm;

        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        public MySpaceView()
        {
            InitializeComponent();
            vm = (MySpaceViewModel)DataContext;
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

        #region 拖拽上传
        private void OnDrop(object sender, DragEventArgs e)
        {
            fileDragMask.Visibility = Visibility.Collapsed;

            var files = e.Data.GetData(DataFormats.FileDrop) as Array;
            foreach (string fileFullName in files)
            {
                FileInfo info = new FileInfo(fileFullName);

                var uploadFileTask = new UploadFileTask
                {
                    Id = Counter.NextUploadTaskID(),
                    FilePath = fileFullName,
                    FileSize = info.Length,
                    FileName = info.Name,
                    FolderId = vm.SelectedFolder.Folder.ID
                };

                // 发布上传任务
                eventAggregator.GetEvent<UploadFileEvent>().Publish(uploadFileTask);
            }
            e.Handled = true;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            fileDragMask.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            fileDragMask.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            fileDragMask.Visibility = Visibility.Visible;
            e.Handled = false;
        }
        #endregion

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
