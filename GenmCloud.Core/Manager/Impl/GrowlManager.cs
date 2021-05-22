using GenmCloud.Core.Event;
using HandyControl.Controls;
using Prism.Events;

namespace GenmCloud.Core.Manager
{
    public class GrowlManager
    {
        private static GrowlManager instance;
        public static GrowlManager GetInstance(IEventAggregator eventAggregator)
        {
            if (instance == null)
            {
                instance = new GrowlManager(eventAggregator);
            }
            return instance;
        }

        private IEventAggregator eventAggregator;

        private GrowlManager(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<GrowlEvent>().Subscribe(GrowlEventActivated);
        }

        private void GrowlEventActivated(string msg)
        {
            Growl.Success("文件保存成功！");
        }

        private void ClearGrowlEventActivated(string token)
        {

        }


    }
}
