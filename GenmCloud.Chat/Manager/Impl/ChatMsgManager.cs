using GenmCloud.ApiService;
using GenmCloud.ApiService.Service;
using GenmCloud.Chat.Tools;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Core.Manager.Impl;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace GenmCloud.Chat.Manager
{
    public interface IChatObjData
    {
        void GetChatObjVOList(uint id);
        void GetChatMsgVOList(uint id);

        void PutChatObjVO(ChatObjVO chatObjVO);
        void PutChatMsgVO(ChatMsgVO chatMsgVO);
    }

    public class ChatObjData
    {
        private ObservableCollection<ChatObjVO> chatObjList;
        private Dictionary<ChatObjVO, ObservableCollection<ChatMsgVO>> chatMsgMap;
        private Dictionary<uint, ChatObjVO> chatObjMap;

        public ChatObjData()
        {
            chatObjList = new ObservableCollection<ChatObjVO>();
            chatMsgMap = new Dictionary<ChatObjVO, ObservableCollection<ChatMsgVO>>();
            chatObjMap = new Dictionary<uint, ChatObjVO>();
        }

        public ObservableCollection<ChatMsgVO> GetChatMsgListByObj(ChatObjVO chatObjVO)
        {
            return chatMsgMap[chatObjVO];
        }

        public ObservableCollection<ChatObjVO> GetChatObjList()
        {
            return chatObjList;
        }

        public void PutChatMsg(ChatObjVO chatObjVO, ChatMsgVO chatMsgVO)
        {
            chatMsgMap[chatObjVO].Add(chatMsgVO);
        }

        public ChatObjVO GetChatObj(uint id)
        {
            return chatObjMap[id];
        }

        public void UpdateLastMsg(uint id, string msg)
        {
            chatObjMap[id].LastMsg = msg;
        }

        public void PutChatObj(ChatObjVO chatObjVO)
        {
            // 如果不存在，即创建。存在即更新
            if (!chatObjMap.ContainsKey(chatObjVO.Id))
            {
                chatObjMap[chatObjVO.Id] = chatObjVO;
                chatMsgMap[chatObjVO] = new ObservableCollection<ChatMsgVO>();
                chatObjList.Add(chatObjVO);
            }
        }
    }

    public class ChatMsgManager : IChatMsgManager
    {
        public class InitChatMsgListBySingleObjEvent : PubSubEvent<uint> { }

        private static ChatMsgManager instance;
        public static ChatMsgManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatMsgManager();
            }
            return instance;
        }

        private ChatObjData chatObjData = new ChatObjData();

        private readonly IEventAggregator eventAggregator;
        private readonly IChatService chatService;

        private ChatMsgManager()
        {
            chatService = NetCoreProvider.Resolve<IChatService>();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();

            // 注册事件监听
            eventAggregator.GetEvent<SendChatMsgEvent>().Subscribe(SendMessage);
            eventAggregator.GetEvent<WebSocketRecvEvent>().Subscribe(ReceiveMessage);
            eventAggregator.GetEvent<InitChatMsgListBySingleObjEvent>().Subscribe(AsyncInitChatMsgListBySingleObj);

            AsyncInit();
        }

        public ObservableCollection<ChatObjVO> GetChatObjList()
        {
            return chatObjData.GetChatObjList();
        }

        public ObservableCollection<ChatMsgVO> GetChatMsgListByObj(ChatObjVO chatObjVO)
        {
            return chatObjData.GetChatMsgListByObj(chatObjVO);
        }

        /// <summary>
        /// TODO 注意线程安全
        /// 异步进行数据的初始化操作
        /// </summary>
        private async void AsyncInit()
        {
            // 获取最近的所有联系对象
            var result = await chatService.GetChatObjIdList();
            if (result.StatusCode == SessionService.RequestOK)
            {
                var chatObjIdList = result.Result;
                if (chatObjIdList == null) return;
                foreach (var id in chatObjIdList)
                {
                    AsyncGetChatObj(id);
                }
            }
        }

        /// <summary>
        ///  通过对应的联系对象
        /// </summary>
        /// <param name="id">联系对象的ID</param>
        public async void AsyncGetChatObj(uint id)
        {
            var result = await chatService.GetChatObj<UserDto>(id);
            if (!ServiceHelper.IsNullOrFail(result))
            {
                var chatObjDto = result.Result;
                UserInfoManager.GetInstance().Store(chatObjDto.Obj.ID, chatObjDto.Obj);
                chatObjData.PutChatObj(ChatObjDto2VOConvert.Convert(chatObjDto));
            }
        }

        /// <summary>
        /// 异步初始化指定的聊天对象的聊天信息列表
        /// </summary>
        /// <param name="id"></param>
        public async void AsyncInitChatMsgListBySingleObj(uint id)
        {
            var chatMsgList = chatObjData.GetChatMsgListByObj(chatObjData.GetChatObj(id));
            if (chatMsgList == null)
            {
                throw new Exception("意料之外的操作");
            }

            // TODO 下列说法，仅存于不断线的情况
            // 如果ChatMsgList不为空，则说明之前已经进行了初始化操作，无需再次进行。
            if (chatMsgList.Count == 0)
            {
                var result = await chatService.GetChatMsgHistoryBySingleObj(id);
                if (result.StatusCode == 200)
                {
                    var chatMsgDtoList = result.Result;
                    foreach (var chatMsgDto in chatMsgDtoList)
                    {
                        var chatMsgVO = ChatMsgDto2VOConvert.Convert(chatMsgDto);
                        if (chatMsgVO.Role == Core.Data.Type.ChatRoleType.Me)
                        {
                            chatMsgVO.User = SessionService.User;
                        }
                        else
                        {
                            chatMsgVO.User = UserInfoManager.GetInstance().Get(id);
                        }
                        chatMsgList.Add(chatMsgVO);
                    }
                }
            }
        }

        public void SendMessage(ChatMsgDto chatMsgDto)
        {
            chatObjData.UpdateLastMsg(chatMsgDto.ReceiverId, chatMsgDto.Data);
            this.eventAggregator.GetEvent<WebSocketSendEvent>().Publish(JsonConvert.SerializeObject(chatMsgDto));
        }

        /// <summary>
        /// 接收服务端的消息，进行反序列化之后，进行相关新信息到来的处理操作
        /// </summary>
        /// <param name="msg"></param>
        private void ReceiveMessage(string msg)
        {
            var chatMsgDto = JsonConvert.DeserializeObject<ChatMsgDto>(msg);

            if (chatMsgDto.Type == 1 && chatMsgDto.SubType != 1)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var chatObjVO = chatObjData.GetChatObj(chatMsgDto.SenderId);
                    chatObjVO.LastMsg = chatMsgDto.Data;

                    var chatMsgVO = ChatMsgDto2VOConvert.Convert(chatMsgDto);
                    chatObjData.PutChatMsg(chatObjVO, chatMsgVO);
                });
            }
        }
    }
}
