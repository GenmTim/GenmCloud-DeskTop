using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace GenmCloud.Storage.ViewModels.ChildView
{
    class MySpaceViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        #region 属性
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

        private string rootFolder;
        public string RootFolder
        {
            get => rootFolder;
            set
            {
                rootFolder = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public DelegateCommand JumpCmd { get; private set; }

        public string GetRootFolder(FolderTreeNodeVO now)
        {
            if (now == null) return "";
            if (now.ParentVO == null) return now.Folder.Name;
            return GetRootFolder(now.ParentVO);
        }

        public MySpaceViewModel()
        {
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.regionManager = NetCoreProvider.Resolve<IRegionManager>();
            this.eventAggregator.GetEvent<UpdateSelectedFolderEvent>().Subscribe(UpdateSelectedFolder);

            // TODO 做跳转命令
            JumpCmd = new DelegateCommand(() => {
            });
        }

        public void UpdateSelectedFolder(object obj)
        {
            if (obj == null) return;
            FolderTreeNodeVO folderContext = (FolderTreeNodeVO)obj;
            SelectedFolder = folderContext;
            RootFolder = GetRootFolder(SelectedFolder);

            eventAggregator.GetEvent<UpdateFileListEvent>().Publish(folderContext.Folder);
        }
    }
}
