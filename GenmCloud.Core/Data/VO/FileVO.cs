using GenmCloud.Core.Event;
using Prism.Mvvm;

namespace GenmCloud.Core.Data.VO
{
    public enum UploadFileState
    {
        Uploading,
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
}
