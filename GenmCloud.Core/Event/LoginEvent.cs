using GenmCloud.Shared.Dto;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Event
{
    public class SignInEvent : PubSubEvent<LoginDto>
    {
        
    }

    public class SignedInEvent : PubSubEvent
    {

    }
}
