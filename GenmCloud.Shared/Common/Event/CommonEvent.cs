using Prism.Events;

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
