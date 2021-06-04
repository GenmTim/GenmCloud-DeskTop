using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class UserInfoUpdateEvent : PubSubEvent { }

    public class ContactListUpdateEvent : PubSubEvent { }

    public class UpdateMenuEvent : PubSubEvent<uint> { }

    // 更新文件夹列表事件
    public class UpdateFolderListEvent : PubSubEvent { }

    // 更新文件列表事件
    public class UpdateFileListEvent : PubSubEvent<object> { }

    // 更新被选中的文件夹
    public class UpdateSelectedFolderEvent : PubSubEvent<object> { }
}
