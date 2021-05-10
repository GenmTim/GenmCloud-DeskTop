using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Core.Manager;
using Prism.Commands;
using Prism.Events;
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

        //private readonly ChatMsgManager chatMsgManager;
        private readonly IEventAggregator eventAggregator;
        public DelegateCommand SendCmd { get; private set; }

        public ChatBoxViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            this.eventAggregator = eventAggregator;
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
            if (string.IsNullOrEmpty(Context.ChatString))
            {
                return;
            }

            string newMsg = Context.ChatString;
            ChatMsgList.Add(new ChatMsgVO { Content = newMsg, Role = Core.Data.Type.ChatRoleType.Me, Type = Core.Data.Type.ChatMessageType.String, Id = 0 });
            ChatMsgList.Add(new ChatMsgVO { Content = newMsg, Role = Core.Data.Type.ChatRoleType.Other, Type = Core.Data.Type.ChatMessageType.String, Id = 1 });
            eventAggregator.GetEvent<SendChatMsgEvent>().Publish(newMsg);
            Context.ChatString = "";
        }
    }
}
