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
        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        public StorageView(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();

            this.dialogHost = dialogHost;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            rightContent.Content = new MySpaceView();

            Loaded += StorageView_Loaded;
            Loaded += InitTask;
        }

        private void InitTask(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeViewItem = (TreeViewItem)folderTreeView.ItemContainerGenerator.ContainerFromIndex(1);
            treeViewItem.IsSelected = true;

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
    }
}
