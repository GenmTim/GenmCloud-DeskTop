using GenmCloud.ApiService;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GenmCloud.Core.Manager
{
    public class ChatMessageManager
    {
        private Dictionary<ChatObjectVO, ObservableCollection<ChatMsgVO>> ChatMsgMap = new Dictionary<ChatObjectVO, ObservableCollection<ChatMsgVO>>();
        private static ChatMessageManager instance;
        private readonly IEventAggregator eventAggregator;

        private static long ChatToken = 0;

        public static long GetChatToken()
        {
            return ++ChatToken;
        }

        // 此种单例写法，可以在Prism注册处的代码更为简单，简洁
        public ChatMessageManager(IEventAggregator eventAggregator)
        {
            if (instance == null)
            {
                instance = this;
                this.eventAggregator = eventAggregator;
                this.eventAggregator.GetEvent<SendChatMsgEvent>().Subscribe(SendMessage);
                this.eventAggregator.GetEvent<WebSocketRecvEvent>().Subscribe(NewMessage);
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

        public void SendMessage(string message)
        {
            this.eventAggregator.GetEvent<WebSocketSendEvent>().Publish(message);
        }

        private void NewMessage(string msg)
        {
            Save(msg);
        }

        // 聊天记录本地化
        private static void Save(string msg)
        {
            CacheManager.GetInstance().SaveRecord("chat", msg);
        }
    }
}
