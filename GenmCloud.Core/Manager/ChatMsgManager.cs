using GenmCloud.Core.Data.VO;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager
{
    public class ChatMsgManager
    {
        private Dictionary<ChatObjectVO, ObservableCollection<ChatMsgVO>> ChatMsgMap = new Dictionary<ChatObjectVO, ObservableCollection<ChatMsgVO>>();
        private static ChatMsgManager instance;
        private readonly IEventAggregator eventAggregator;

        private static long ChatToken = 0;

        public static long GetChatToken()
        {
            return ++ChatToken;
        }

        // 此种单例写法，可以在Prism注册处的代码更为简单，简洁
        public ChatMsgManager(IEventAggregator eventAggregator)
        {
            if (instance == null)
            {
                instance = this;
                this.eventAggregator = eventAggregator;
            }
            else
            {
                throw new Exception("违规的的构造行为");
            }
        }

        public ObservableCollection<ChatMsgVO> GetChatMsgList(ChatObjectVO objectVO)
        {
            if (!ChatMsgMap.ContainsKey(objectVO))
            {
                ChatMsgMap[objectVO] = new ObservableCollection<ChatMsgVO>();
            }
            return ChatMsgMap[objectVO];
        }
    }
}
