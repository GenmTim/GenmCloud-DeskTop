using GenmCloud.Shared.Dto;
using Prism.Events;

namespace GenmCloud.Core.Event
{
    public class SignInEvent : PubSubEvent<LoginDto>
    {

    }

    public class SignedInEvent : PubSubEvent
    {

    }

    public class SignUpEvent : PubSubEvent<LoginDto> 
    {
    }
}
