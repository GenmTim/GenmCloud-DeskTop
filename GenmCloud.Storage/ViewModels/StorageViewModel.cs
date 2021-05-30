using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GenmCloud.Storage.ViewModels
{
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

        private ObservableCollection<FileItemVO> fileItemVOList = new ObservableCollection<FileItemVO>();
        public ObservableCollection<FileItemVO> FileItemVOList { get => fileItemVOList; set => fileItemVOList = value; }

        public StorageViewModel(IDialogHostService dialogHost)
        {
            this.dialogHost = dialogHost;
            this.AddNewFolderCmd = new DelegateCommand<object>(AddNewFolder);
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
            var optList = new ObservableCollection<MenuItem> { new MenuItem { Header = "新建文件夹", Command = AddNewFolderCmd } };

            var child1 = new FolderTreeNodeItemVO
            {
                Id = 11,
                Name = "测试1",
                OptCmdList = optList,
                Children = null,
            };

            var child2 = new FolderTreeNodeItemVO
            {
                Id = 12,
                Name = "测试2",
                Children = null,
            };

            FileTreeNodeItemList.Add(new FolderTreeNodeItemVO
            {
                Id = 1,
                Name = "测试",
                Children = new ObservableCollection<FolderTreeNodeItemVO>(),
            });
            FileTreeNodeItemList[0].Children.Add(child1);
            FileTreeNodeItemList[0].Children.Add(child2);

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
