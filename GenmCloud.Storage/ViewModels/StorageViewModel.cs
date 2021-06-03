using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.Storage.ViewModels
{
    public static class UpdateDataHelper
    {
        public static void UpdateFolderListVO(ObservableCollection<FolderTreeNodeItemVO> folderTreeList, List<FolderListDto> newTreeFolderList)
        {
            //var optList = new ObservableCollection<MenuItem> { new MenuItem { Header = "新建文件夹", Command = AddNewFolderCmd } };

            var newTreeFolders = newTreeFolderList.ToArray();

            for (int i = 0; i < newTreeFolders.Length; ++i)
            {
                if (i <= folderTreeList.Count - 1)
                {
                    folderTreeList[i].Id = newTreeFolderList[i].Folder.ID;
                    folderTreeList[i].Name = newTreeFolderList[i].Folder.Name;
                }
                else
                {
                    folderTreeList.Add(new FolderTreeNodeItemVO
                    {
                        Name = newTreeFolders[i].Folder.Name,
                        Id = newTreeFolders[i].Folder.ID,
                        //OptCmdList =
                    });
                }

                if (newTreeFolderList[i].Children != null)
                {
                    if (folderTreeList[i].Children == null)
                    {
                        folderTreeList[i].Children = new ObservableCollection<FolderTreeNodeItemVO>();
                    }
                    UpdateFolderListVO(folderTreeList[i].Children, newTreeFolderList[i].Children);
                }
                else
                {
                    if (folderTreeList[i].Children != null)
                    {
                        folderTreeList[i].Children = null;
                    }
                }
            }

            for (int i = folderTreeList.Count - 1; i > newTreeFolders.Length - 1; --i)
            {
                folderTreeList.RemoveAt(i);
            }
        }
    }

    public class FolderTreeNodeItemVO : BindableBase
    {
        private uint id;
        public uint Id
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<FolderTreeNodeItemVO> children;
        public ObservableCollection<FolderTreeNodeItemVO> Children
        {
            get => children;
            set
            {
                children = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MenuItem> optCmdList;
        public ObservableCollection<MenuItem> OptCmdList
        {
            get => optCmdList;
            set
            {
                optCmdList = value;
                RaisePropertyChanged();
            }
        }
    }

    public class FileItemVO
    {
        public uint Id { get; set; }
        public string FileName { get; set; }
        public string OwnerName { get; set; }
        public string CreatedAt { get; set; }
    }


    public class StorageViewModel : BindableBase
    {
        public DelegateCommand<object> AddNewFolderCmd { get; private set; }

        private ObservableCollection<FolderTreeNodeItemVO> fileTreeNodeItemList = new ObservableCollection<FolderTreeNodeItemVO>();
        public ObservableCollection<FolderTreeNodeItemVO> FileTreeNodeItemList
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

        private ObservableCollection<FileItemVO> fileItemVOList = new ObservableCollection<FileItemVO>();
        public ObservableCollection<FileItemVO> FileItemVOList { get => fileItemVOList; set => fileItemVOList = value; }

        public StorageViewModel(IDialogHostService dialogHost)
        {
            this.dialogHost = dialogHost;
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.folderService = NetCoreProvider.Resolve<IFolderService>();
            this.AddNewFolderCmd = new DelegateCommand<object>(AddNewFolder);
            this.eventAggregator.GetEvent<UpdateFolderListEvent>().Subscribe(async () =>
            {
                var result = await folderService.GetForlderList();
                if (result.StatusCode == 200)
                {
                    List<FolderListDto> treeDeptList = result.Result;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (treeDeptList == null)
                        {
                            FileTreeNodeItemList = null;
                            return;
                        }

                        if (treeDeptList != null && FileTreeNodeItemList == null)
                        {
                            FileTreeNodeItemList = new ObservableCollection<FolderTreeNodeItemVO>();
                        }
                        UpdateDataHelper.UpdateFolderListVO(FileTreeNodeItemList, treeDeptList);
                    });
                }
            });
            Simulation();
        }

        private async void AddNewFolder(object obj)
        {
            uint id;
            if (obj == null)
            {
                id = 0;
            }
            else
            {
                id = (uint)obj;
            }

            var result = await DialogHelper.ShowInputValueDialog(dialogHost, "StorageRoot", "添加新文件夹", "添加", "取消", "请输入新的文件夹名字");
            if (result != null && result.Result == ButtonResult.OK)
            {
                string folderName = result.Parameters.GetValue<string>("value");
                //AddNewFolderToServer(folderName);
            }
        }

        public void Simulation()
        {

            //var child1 = new FolderTreeNodeItemVO
            //{
            //    Id = 11,
            //    Name = "测试1",
            //    OptCmdList = optList,
            //    Children = null,
            //};

            //var child2 = new FolderTreeNodeItemVO
            //{
            //    Id = 12,
            //    Name = "测试2",
            //    Children = null,
            //};

            //FileTreeNodeItemList.Add(new FolderTreeNodeItemVO
            //{
            //    Id = 1,
            //    Name = "测试",
            //    Children = new ObservableCollection<FolderTreeNodeItemVO>(),
            //});
            //FileTreeNodeItemList[0].Children.Add(child1);
            //FileTreeNodeItemList[0].Children.Add(child2);

            FileItemVOList.Add(new FileItemVO { FileName = "任务", OwnerName = "蔡承龙", CreatedAt = "3月22日 21:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "行业分类——2018国家统计局数据_行业分类——2018国家统计局数据", OwnerName = "余浩臻", CreatedAt = "3月22日 23:15" });
            FileItemVOList.Add(new FileItemVO { FileName = "牛人认证", OwnerName = "蔡承龙", CreatedAt = "3月22日 23:12" });
            FileItemVOList.Add(new FileItemVO { FileName = "企业认证关键字段", OwnerName = "蔡承龙", CreatedAt = "3月22日 13:32" });
            FileItemVOList.Add(new FileItemVO { FileName = "企业认证", OwnerName = "余浩臻", CreatedAt = "3月22日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
            FileItemVOList.Add(new FileItemVO { FileName = "汇报、考评关键字", OwnerName = "余浩臻", CreatedAt = "3月15日 19:35" });
        }

    }
}
