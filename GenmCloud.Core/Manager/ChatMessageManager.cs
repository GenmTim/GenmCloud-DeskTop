using GenmCloud.ApiService;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Newtonsoft.Json;
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

        private static uint ChatToken = 0;

        public static uint GetChatToken()
        {
            return ++ChatToken;
        }

        public static ChatMessageManager GetInstance()
        {
            if (instance == null)
            {
                IEventAggregator eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
                instance = new ChatMessageManager(eventAggregator);
            }
            return instance;

        }

        // 此种单例写法，可以在Prism注册处的代码更为简单，简洁
        private ChatMessageManager(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SendChatMsgEvent>().Subscribe(SendMessage);
            this.eventAggregator.GetEvent<WebSocketRecvEvent>().Subscribe(NewMessage);
        }

        public ObservableCollection<ChatMsgVO> GetChatMsgList(ChatObjectVO objectVO)
        {
            if (!ChatMsgMap.ContainsKey(objectVO))
            {
                ChatMsgMap[objectVO] = new ObservableCollection<ChatMsgVO>();
            }
            return ChatMsgMap[objectVO];
        }

        public void SendMessage(ChatDto chatDto)
        {
            this.eventAggregator.GetEvent<WebSocketSendEvent>().Publish(JsonConvert.SerializeObject(chatDto));
        }

        private void NewMessage(string msg)
        {
            // 转换为通用的对象
            var chatDto = JsonConvert.DeserializeObject<ChatDto>(msg);
            var token = new ChatObjectVO { Id = chatDto.SenderId };
            if (!ChatMsgMap.ContainsKey(token))
            {
                ChatMsgMap[token] = new ObservableCollection<ChatMsgVO>();
            }

            Uri avatar = null;
            if (AvatarManager.AvatarMap.ContainsKey(chatDto.SenderId))
            {
                avatar = AvatarManager.AvatarMap[chatDto.SenderId];
            }

            ChatMsgMap[token].Add(new ChatMsgVO
            {
                Id = chatDto.Id,
                Content = chatDto.Data,
                Role = Data.Type.ChatRoleType.Other,
                Type = Data.Type.ChatMessageType.String,
                Avatar = avatar,
            });
        }

        // 聊天记录本地化
        private static void Save(string msg)
        {
            CacheManager.GetInstance().SaveRecord("chat", msg);
        }
    }
}
