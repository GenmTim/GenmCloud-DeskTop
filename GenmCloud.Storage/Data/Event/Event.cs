using GenmCloud.Storage.Data.Task;
using Prism.Events;

namespace GenmCloud.Storage.Data.Event
{
    // 上传文件事件
    public class UploadFileEvent : PubSubEvent<UploadFileTask> {}

    // 文件传输完成事件
    public class UploadedFileEvent : PubSubEvent<UploadFileTask> {}

    // 更新文件夹列表事件
    public class UpdateFolderListEvent : PubSubEvent { }

    // 更新文件列表事件
    public class UpdateFileListEvent : PubSubEvent<object> { }

    // 更新被选中的文件夹
    public class UpdateSelectedFolderEvent : PubSubEvent<object> { }

    // 更新文件列表事件
    public class DownloadFileEvent : PubSubEvent<DownloadFileTask> { }
}
