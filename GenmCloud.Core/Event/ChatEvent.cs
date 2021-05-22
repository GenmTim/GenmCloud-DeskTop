using GenmCloud.Shared.Dto;
using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class SendChatMsgEvent : PubSubEvent<ChatMsgDto>
    {
    }
}
