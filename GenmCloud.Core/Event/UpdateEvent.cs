using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class UserInfoUpdateEvent : PubSubEvent { }

    public class ContactListUpdateEvent : PubSubEvent { }

    public class UpdateMenuEvent : PubSubEvent<uint> { }
}
