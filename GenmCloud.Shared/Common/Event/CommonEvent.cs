using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Event
{
    public class GrowlObj
    {
        public string msg;
    }



    public class GrowlEvent : PubSubEvent<string>
    {
    }

    public class ClearGrowlEvent : PubSubEvent<string> { }
}
