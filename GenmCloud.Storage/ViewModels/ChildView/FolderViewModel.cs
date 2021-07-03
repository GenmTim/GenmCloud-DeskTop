using Genm.WPF.Data.Event;
using GenmCloud.ApiService.Service;
using GenmCloud.ApiService.Service.Impl;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using GenmCloud.Storage.Data.Event;
using GenmCloud.Storage.Data.Task;
using GenmCloud.Storage.Data.VO;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenmCloud.Storage.ViewModels.ChildView
{
    class FolderViewModel : BindableBase, INavigationAware
    {
        private FolderDto context;
        public FolderDto Context
        {
            get => context;
            set
            {
                context = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<FileItemVO> fileList = new ObservableCollection<FileItemVO>();

        public ObservableCollection<FileItemVO> FileList
        {
            get => fileList;
            set
            {
                fileList = value;
                RaisePropertyChanged();
            }
        }

        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IFileService fileService;
        private readonly IFolderService folderService;

        public DelegateCommand<object> DownloadCmd { get; private set; }
        public DelegateCommand<object> DeleteCmd { get; private set; }

        public FolderViewModel()
        {
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.folderService = NetCoreProvider.Resolve<IFolderService>();
            this.regionManager = NetCoreProvider.Resolve<IRegionManager>();
            this.fileService = NetCoreProvider.Resolve<IFileService>();
            eventAggregator.GetEvent<UpdateFileListEvent>().Subscribe(UpdateFileList);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Context = navigationContext.Parameters.GetValue<FolderDto>("context");
            eventAggregator.GetEvent<UpdateFileListEvent>().Publish(null);
            this.DownloadCmd = new DelegateCommand<object>(Download);
            this.DeleteCmd = new DelegateCommand<object>(Delete);
        }

        public async void UpdateFileList(object newContext)
        {
            if (Context == null) return;

            var res = await folderService.GetFileListByFolder(context.ID);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                if (res.Result.Count != 0)
                {
                    FileList ??= new ObservableCollection<FileItemVO>();
                }
                else
                {
                    FileList = null;
                    return;
                }

                var newFileList = res.Result;
                Dictionary<uint, object> dataMap = new Dictionary<uint, object>();
                foreach (var fileDto in newFileList)
                {
                    dataMap[fileDto.Id] = fileDto;
                }

                // 循环遍历旧列表，存在的更新，不存在的替换，没记录的追加
                for (int i = fileList.Count - 1; i >= 0; i--)
                {
                    if (!dataMap.ContainsKey(fileList[i].ID))
                    {
                        // 不存在则删除
                        fileList.Remove(fileList[i]);
                    }
                    else
                    {
                        var fileVO = fileList[i];
                        var newData = (FileDto)dataMap[fileList[i].ID];
                        fileVO.Name = newData.Name;
                        dataMap.Remove(fileList[i].ID);
                    }
                }

                // 剩余数据为新的额外数据
                foreach (var pair in dataMap)
                {
                    var fileDto = (FileDto)pair.Value;
                    var vo = new FileItemVO
                    {
                        ID = fileDto.Id,
                        Name = fileDto.Name,
                        OwnerName = fileDto.OwnerName,
                        Size = fileDto.Size,
                        LastUpdatedTime = "蔡承龙 最后更新于 3月22日 23:15",
                        CreatedAt = fileDto.CreatedAt,
                    };
                    if (!string.IsNullOrEmpty(fileDto.ThumbAddr))
                    {
                        var source = new BitmapImage(new Uri(fileDto.ThumbAddr));
                        vo.Thumb = new ImageBrush { ImageSource = source, Stretch = Stretch.UniformToFill };
                    }
                    AttachContextMenu(vo);
                    FileList.Add(vo);
                }
            }
            if (FileList == null || FileList.Count == 0) FileList = null;
        }

        private void Download(object obj)
        {
            var vo = (FileItemVO)(obj);
            string storagePath;
            
            // 选择下载的存放路径
            var openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                storagePath = openFileDialog.SelectedPath;
            } else
            {
                return;
            }

            // 开启下载任务
            eventAggregator.GetEvent<DownloadFileEvent>().Publish(new DownloadFileTask { FileID = vo.ID, FileSize = vo.Size, FileName = vo.Name, TargetAddr=storagePath });
        }

        private void Delete(object obj)
        {
            var vo = (FileItemVO)(obj);

            fileService.DeleteFile(vo.ID);
            eventAggregator.GetEvent<ToastShowEvent>().Publish("成功删除文件");
        }

        private void AttachContextMenu(FileItemVO vo)
        {
            vo.ContextMenus = new ObservableCollection<MenuItem>();
            var contextMenus = vo.ContextMenus;
            contextMenus.Add(new MenuItem { Header = "下载", Command=DownloadCmd, CommandParameter=vo });
            contextMenus.Add(new MenuItem { Header = "删除", Command=DeleteCmd, CommandParameter=vo });
        }
    }
}
