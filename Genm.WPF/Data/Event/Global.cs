using Prism.Events;

namespace Genm.WPF.Data.Event
{
    public class ToastShowEvent : PubSubEvent<string> { }

    public class RunGlobalProgressEvent : PubSubEvent<bool> { }
}
