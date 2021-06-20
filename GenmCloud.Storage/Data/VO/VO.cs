using GenmCloud.Core.Event;
using GenmCloud.Shared.Dto;
using GenmCloud.Storage.Data.Task;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace GenmCloud.Storage.Data.VO
{

    public class FolderTreeNodeVO : BindableBase
    {
        public string Icon { get; set; }

        private FolderDto folder;
        public FolderDto Folder
        {
            get => folder;
            set
            {
                folder = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<FolderTreeNodeVO> children;
        public ObservableCollection<FolderTreeNodeVO> Children
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

        public string Tag { get; set; }

        public FolderTreeNodeVO ParentVO;
    }

    public class FileItemVO
    {
        public uint ID { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public ImageBrush Thumb { get; set; }
        public string LastUpdatedTime { get; set; }
        public string CreatedAt { get; set; }
        public ObservableCollection<MenuItem> ContextMenus { get; set; }
    }

    public enum UploadFileState
    {
        Uploading,
        Pause,
        Success,
        Fail,
    }

    public enum DownloadFileState
    { 
        Downloading,
        Pause,
        Success,
        Fail,
    }

    public class UploadFileItemVO : BindableBase
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

        private UploadFileState state;
        public UploadFileState State
        {
            get => state;
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }

        private long uploadedSize;
        public long UploadedSize
        {
            get => uploadedSize;
            set
            {
                uploadedSize = value;
                Rate = uploadedSize * 100 / Size;
                RaisePropertyChanged();
            }
        }

        private double rate;
        public double Rate
        {
            get => rate;
            set
            {
                rate = value;
                RaisePropertyChanged();
            }
        }

        public UploadFileTask Task;
    }

    public class DownloadFileItemVO : BindableBase
    {
        public uint Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

        private DownloadFileState state;
        public DownloadFileState State
        {
            get => state;
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }

        private long downloadedSize;
        public long DownloadedSize
        {
            get => downloadedSize;
            set
            {
                downloadedSize = value;
                Rate = downloadedSize * 100 / Size;
                RaisePropertyChanged();
            }
        }

        private double rate;
        public double Rate
        {
            get => rate;
            set
            {
                rate = value;
                RaisePropertyChanged();
            }
        }

        public DownloadFileTask Task;
    }
}
