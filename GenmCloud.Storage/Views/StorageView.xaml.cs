using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Storage.Data.Event;
using GenmCloud.Storage.Data.Task;
using GenmCloud.Storage.Data.VO;
using GenmCloud.Storage.ViewModels;
using Prism.Events;
using Prism.Regions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.Storage.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly StorageViewModel vm;

        public StorageView(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();
            vm = (StorageViewModel)DataContext;

            this.dialogHost = dialogHost;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            Loaded += StorageView_Loaded;
            Loaded += InitTask;
        }

        private void InitTask(object sender, RoutedEventArgs e)
        {
            //TreeViewItem treeViewItem = (TreeViewItem)folderTreeView.ItemContainerGenerator.ContainerFromIndex(1);
            //treeViewItem.IsSelected = true;

            Loaded -= InitTask;
        }

        private void StorageView_Loaded(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<UpdateFolderListEvent>().Publish();
        }

        private void FolderItemChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeNode = (FolderTreeNodeVO)e.NewValue;
            if (treeNode.Folder == null) return;
            eventAggregator.GetEvent<UpdateSelectedFolderEvent>().Publish(treeNode);
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
                    ID = Counter.NextUploadTaskID(),
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
    }
}
