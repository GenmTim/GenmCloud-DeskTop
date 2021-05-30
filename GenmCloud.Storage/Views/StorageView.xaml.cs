using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Core.UserControls.Common.Views;
using GenmCloud.Storage.Cmd;
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

        public StorageView(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            this.dialogHost = dialogHost;
        }

        private readonly IEventAggregator eventAggregator;

        private void OnDrop(object sender, DragEventArgs e)
        {
            fileDragMask.Visibility = Visibility.Collapsed;
            var files = e.Data.GetData(DataFormats.FileDrop) as Array;
            foreach (string fileFullName in files)
            {
                FileInfo info = new FileInfo(fileFullName);

                var uploadFileTask = new UploadFileTask
                {
                    FilePath = fileFullName,
                    FileSize = info.Length,
                    FileName = info.Name
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

        private void CheckToGridView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("WrapModelViewer") as Style;
        }

        private void CheckToListView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("GridViewModelViewer") as Style;
        }

        private void ShrinkGrid(object sender, RoutedEventArgs e)
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

        private void OpenGrid(object sender, RoutedEventArgs e)
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

        private void _content_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
