using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class UploadFileTask
    {
        public string FilePath;
        public string FileName;
        public long FileSize;
    }



    public class UploadFileEvent : PubSubEvent<UploadFileTask>
    {
    }

    public class UploadedFileEvent : PubSubEvent<UploadFileTask>
    {
    }
}
