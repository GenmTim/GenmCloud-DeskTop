using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genm.WPF.Data.Event
{
    public class ToastShowEvent : PubSubEvent<string> { }

    public class RunGlobalProgressEvent : PubSubEvent<bool> { }
}
