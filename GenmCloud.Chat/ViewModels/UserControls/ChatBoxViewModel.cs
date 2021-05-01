﻿using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Manager;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace GenmCloud.Chat.ViewModels.UserControls
{
    public class ChatBoxViewModel : BindableBase
    {
        private ChatObjectVO context;
        public ChatObjectVO Context
        {
            get => context;
            set
            {
                context = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ChatMsgVO> chatMsgList;
        public ObservableCollection<ChatMsgVO> ChatMsgList
        {
            get => chatMsgList;
            set
            {
                chatMsgList = value;
                RaisePropertyChanged();
            }
        }

        private readonly ChatMsgManager chatMsgManager;
        public DelegateCommand SendCmd { get; private set; }

        public ChatBoxViewModel(IContainerProvider containerProvider)
        {
            this.SendCmd = new DelegateCommand(SendMsg);
            ChatMsgList = new ObservableCollection<ChatMsgVO>();
            //this.chatMsgManager = containerProvider.Resolve<ChatMsgManager>();
            Simulation();
        }

        private void Simulation()
        {
            Context = new ChatObjectVO
            {
                Name = "测试",
                Id = 1,
            };

            //ChatMsgList = chatMsgManager.GetChatMsgList(ChatObject);
        }

        private void SendMsg()
        {
            string newMsg = Context.ChatString;
            ChatMsgList.Add(new ChatMsgVO { Content=newMsg, Role=Core.Data.Type.ChatRoleType.Me, Type=Core.Data.Type.ChatMessageType.String, Id=0});
            ChatMsgList.Add(new ChatMsgVO { Content=newMsg, Role=Core.Data.Type.ChatRoleType.Other, Type=Core.Data.Type.ChatMessageType.String, Id=1});

            Context.ChatString = "";
        }
    }
}