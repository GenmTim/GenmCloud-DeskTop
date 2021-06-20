using GenmCloud.ApiService.Service;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using GenmCloud.Storage.Data.Event;
using GenmCloud.Storage.Data.VO;
using GenmCloud.Storage.Views.ChildView;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Storage.ViewModels
{
    public class StorageViewModel : BindableBase
    {
        public DelegateCommand<object> AddFolderCmd { get; private set; }

        private FolderTreeNodeVO selectedFolder;
        public FolderTreeNodeVO SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateSelectedFolder(object obj)
        {
            if (obj == null) return;
            FolderTreeNodeVO folderContext = (FolderTreeNodeVO)obj;
            SelectedFolder = folderContext;
            eventAggregator.GetEvent<UpdateFileListEvent>().Publish(folderContext.Folder);

            if (SelectedFolder.Tag == "Root")
            {
                switch (SelectedFolder.Folder.Name)
                {
                    case "主页":
                        RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(HomeView));
                        break;
                    case "我的空间":
                        RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(MySpaceView));
                        break;
                    case "共享空间":
                        RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(ShareSpaceView));
                        break;
                    case "收藏夹":
                        RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(FavoriteView));
                        break;
                    case "回收站":
                        RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(TrashView));
                        break;
                }
            }
            else
            {
                var param = new NavigationParameters
                {
                    { "context", SelectedFolder.Folder }
                };
                RegionHelper.RequestNavigate(regionManager, RegionToken.StorageRightContent, typeof(FolderView), param);
            }
        }


        private ObservableCollection<FolderTreeNodeVO> fileTreeNodeItemList = new ObservableCollection<FolderTreeNodeVO>();
        public ObservableCollection<FolderTreeNodeVO> FileTreeNodeItemList
        {
            get => fileTreeNodeItemList;
            set
            {
                fileTreeNodeItemList = value;
                RaisePropertyChanged();
            }
        }
        private readonly IDialogHostService dialogHost;
        private readonly IEventAggregator eventAggregator;
        private readonly IFolderService folderService;
        private readonly IRegionManager regionManager;

        private ObservableCollection<FileItemVO> fileItemVOList = new ObservableCollection<FileItemVO>();
        public ObservableCollection<FileItemVO> FileItemVOList { get => fileItemVOList; set => fileItemVOList = value; }

        public StorageViewModel(IDialogHostService dialogHost)
        {
            this.dialogHost = dialogHost;
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.folderService = NetCoreProvider.Resolve<IFolderService>();
            this.regionManager = NetCoreProvider.Resolve<IRegionManager>();
            this.AddFolderCmd = new DelegateCommand<object>(AddFolder);

            InitTreeRootNode();
            this.eventAggregator.GetEvent<UpdateSelectedFolderEvent>().Subscribe(UpdateSelectedFolder);
            this.eventAggregator.GetEvent<UpdateFolderListEvent>().Subscribe(UpdateFolderList);
        }

        private void InitTreeRootNode()
        {
            FileTreeNodeItemList.Add(new FolderTreeNodeVO { Folder = new FolderDto { Name = "主页", ParentID = uint.MaxValue, }, Icon = "\xe982", Tag = "Root" });
            FileTreeNodeItemList.Add(new FolderTreeNodeVO { Folder = new FolderDto { Name = "我的空间", ParentID = uint.MaxValue }, Icon = "\xe980", Children = new ObservableCollection<FolderTreeNodeVO>(), Tag = "Root" });
            FileTreeNodeItemList.Add(new FolderTreeNodeVO { Folder = new FolderDto { Name = "共享空间", ParentID = uint.MaxValue }, Icon = "\xe97a", Children = new ObservableCollection<FolderTreeNodeVO>(), Tag = "Root" });
            FileTreeNodeItemList.Add(new FolderTreeNodeVO { Folder = new FolderDto { Name = "收藏夹", ParentID = uint.MaxValue }, Icon = "\xe69c", Tag = "Root" });
            FileTreeNodeItemList.Add(new FolderTreeNodeVO { Folder = new FolderDto { Name = "回收站", ParentID = uint.MaxValue }, Icon = "\xe861", Tag = "Root" });
        }

        public async void AddFolder(object obj)
        {
            uint parentId = 0;
            if (obj != null)
            {
                parentId = (uint)obj;
            }

            var result = await DialogHelper.ShowInputValueDialog(dialogHost, "StorageRoot", "添加新文件夹", "添加", "取消", "请输入新的文件夹名字");
            if (result != null && result.Result == ButtonResult.OK)
            {
                string folderName = result.Parameters.GetValue<string>("value");
                var res = await folderService.CreateFolder(parentId, folderName);
                if (res.StatusCode == ServiceHelper.RequestOk)
                {
                    eventAggregator.GetEvent<UpdateFolderListEvent>().Publish();
                }
            }
        }

        public async void UpdateFolderList()
        {
            var result = await folderService.GetForlderList();
            if (result.StatusCode == ServiceHelper.RequestOk)
            {
                List<FolderListDto> treeDeptList = result.Result;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (treeDeptList == null)
                    {
                        FileTreeNodeItemList[1].Children = null;
                        return;
                    }

                    if (treeDeptList != null && FileTreeNodeItemList[1].Children == null)
                    {
                        FileTreeNodeItemList[1].Children = new ObservableCollection<FolderTreeNodeVO>();
                    }
                    UpdateFolderListVO(FileTreeNodeItemList[1], treeDeptList);
                });
            }
        }

        public void UpdateFolderListVO(FolderTreeNodeVO folderTreeNode, List<FolderListDto> newTreeFolderList)
        {
            var newTreeFolders = newTreeFolderList.ToArray();

            for (int i = 0; i < newTreeFolders.Length; ++i)
            {
                if (i <= folderTreeNode.Children.Count - 1)
                {
                    folderTreeNode.Children[i].Folder = newTreeFolderList[i].Folder;
                }
                else
                {
                    folderTreeNode.Children.Add(new FolderTreeNodeVO
                    {
                        Folder = newTreeFolders[i].Folder,
                        Icon = "\xe645",
                        ParentVO = folderTreeNode,
                        OptCmdList = new ObservableCollection<MenuItem> { new MenuItem { Header = "新建文件夹", Command = AddFolderCmd, CommandParameter = newTreeFolders[i].Folder.ID } },
                    });
                }

                if (newTreeFolderList[i].Children != null)
                {
                    if (folderTreeNode.Children[i].Children == null)
                    {
                        folderTreeNode.Children[i].Children = new ObservableCollection<FolderTreeNodeVO>();
                    }
                    UpdateFolderListVO(folderTreeNode.Children[i], newTreeFolderList[i].Children);
                }
                else
                {
                    if (folderTreeNode.Children[i].Children != null)
                    {
                        folderTreeNode.Children[i].Children = null;
                    }
                }
            }

            for (int i = folderTreeNode.Children.Count - 1; i > newTreeFolders.Length - 1; --i)
            {
                folderTreeNode.Children.RemoveAt(i);
            }
        }
    }
}
