using Prism.Mvvm;

namespace GenmCloud.Core.Data.VO
{
    public enum UploadFileState
    {
        上传中,
        暂停,
        上传完成,
        上传失败,
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

    }
}
