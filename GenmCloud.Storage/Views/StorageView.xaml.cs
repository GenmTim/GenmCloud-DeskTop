using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Core.UserControls.Cmd;
using GenmCloud.Core.UserControls.Common.Views;
using GenmCloud.Storage.ViewModels;
using GenmCloud.Storage.Views.ChildView;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GenmCloud.Storage.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        public class ShrinkGridEvent : PubSubEvent { }
        public class ExpandGridEvent : PubSubEvent { }

        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;

        public StorageView(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            this.dialogHost = dialogHost;
            Loaded += StorageView_Loaded;
            regionManager.RegisterViewWithRegion(RegionToken.FileListContent, typeof(FileListView));
        }

        private void StorageView_Loaded(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<UpdateFolderListEvent>().Publish();
            eventAggregator.GetEvent<ShrinkGridEvent>().Subscribe(ShrinkGrid);
            eventAggregator.GetEvent<ExpandGridEvent>().Subscribe(ExpandGrid);
        }

        #region 拖拽上传
        private void OnDrop(object sender, DragEventArgs e)
        {
            fileDragMask.Visibility = Visibility.Collapsed;
            if (((StorageViewModel)DataContext).SelectedFolder == null)
            {
                return;
            }

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
                    FolderId = ((StorageViewModel)DataContext).SelectedFolder.ID
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

        private void FolderItemChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeNode = (FolderTreeNodeVO)e.NewValue;
            if (treeNode.Folder == null) return;
            eventAggregator.GetEvent<UpdateSelectedFolderEvent>().Publish(treeNode.Folder);
        }
    }
}
