using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class UploadFileTask
    {
        public uint Id;
        public string FilePath;
        public string FileName;
        public long FileSize;
        public uint FolderId;
    }



    public class UploadFileEvent : PubSubEvent<UploadFileTask>
    {
    }

    public class UploadedFileEvent : PubSubEvent<UploadFileTask>
    {
    }
}
