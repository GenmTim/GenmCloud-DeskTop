using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class ShowNameCardEvent : PubSubEvent<uint> { }

    public class UpdateNameCardContextEvent : PubSubEvent<uint> { }
}
