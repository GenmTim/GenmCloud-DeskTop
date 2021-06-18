using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenmCloud.Storage.ViewModels.ChildView.SubItem
{
    public class FileItemVO
    {
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string Thumb { get; set; }
        public string LastUpdatedTime { get; set; }
        public string CreatedAt { get; set; }
    }

    class FileListViewModel : BindableBase
    {
        private FolderDto context;

        private ObservableCollection<FileItemVO> fileList;

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
        private readonly IFolderService folderService;

        public FileListViewModel()
        {
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            folderService = NetCoreProvider.Resolve<IFolderService>();
            eventAggregator.GetEvent<UpdateFileListEvent>().Subscribe(UpdateFileList);
        }

        public async void UpdateFileList(object newContext)
        {
            if (newContext == null)
            {
                if (context == null) return;
            }
            else
            {
                context = (FolderDto)newContext;
            }

            var res = await folderService.GetFileListByFolder(context.ID);
            if (res.StatusCode == ServiceHelper.RequestOk)
            {
                FileList?.Clear();
                if (res.Result.Count != 0) {
                    FileList ??= new ObservableCollection<FileItemVO>();
                }
                foreach (var fileDto in res.Result)
                {
                    var vo = new FileItemVO
                    {
                        Name = fileDto.Name,
                        OwnerName = fileDto.OwnerName,
                        LastUpdatedTime = "蔡承龙 最后更新于 3月22日 23:15",
                        CreatedAt = fileDto.CreatedAt,
                    };
                    if (!string.IsNullOrEmpty(fileDto.ThumbAddr))
                    {
                        vo.Thumb = fileDto.ThumbAddr;
                    }
                    FileList.Add(vo);
                }
            }
            if (FileList != null &&  FileList.Count == 0) FileList = null;
            //FileList ??= new ObservableCollection<FileItemVO>()
            //{
            //    new FileItemVO 
            //    { 
            //        Name = "ASFJFOJSI.exe",
            //        OwnerName="蔡承龙",
            //        LastUpdatedTime="蔡承龙 最后更新于 3月22日 23:15",
            //    },
            //    new FileItemVO 
            //    {
            //        Name = "BADUAD.txt",
            //        OwnerName="余浩臻",
            //        LastUpdatedTime="余浩臻 最后更新于 3月22日 23:15",
            //    },
            //    new FileItemVO 
            //    {
            //        Name = "ASVA.jpg",
            //        OwnerName="蔡承龙",
            //        LastUpdatedTime="蔡承龙 最后更新于 3月22日 23:15",
            //    },
            //};
        }

    }
}
