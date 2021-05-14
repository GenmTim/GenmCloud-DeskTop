using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Core.Manager;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace GenmCloud.Chat.ViewModels.UserControls
{
    public class ChatBoxViewModel : BindableBase, INavigationAware
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

        private readonly ChatMessageManager chatMsgManager;
        private readonly IEventAggregator eventAggregator;
        public DelegateCommand SendCmd { get; private set; }

        public ChatBoxViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            this.eventAggregator = eventAggregator;
            this.SendCmd = new DelegateCommand(SendMsg);
            ChatMsgList = new ObservableCollection<ChatMsgVO>();
            this.chatMsgManager = containerProvider.Resolve<ChatMessageManager>();
            Simulation();
        }

        private void Simulation()
        {
            Context = new ChatObjectVO
            {
                Name = "测试",
                Id = 1,
            };

            var chatList = chatMsgManager.GetChatMsgList(Context);

            chatList.Add(new ChatMsgVO { Content = "测试消息", Role = Core.Data.Type.ChatRoleType.Me, Type = Core.Data.Type.ChatMessageType.String, Id = 0 });
            chatList.Add(new ChatMsgVO { Content = "测试消息", Role = Core.Data.Type.ChatRoleType.Other, Type = Core.Data.Type.ChatMessageType.String, Id = 1 });
        }

        private void SendMsg()
        {
            if (string.IsNullOrEmpty(Context.ChatString))
            {
                return;
            }
            string newMsg = Context.ChatString;

            ChatMsgList.Add(new ChatMsgVO { Content = "测试消息", Role = Core.Data.Type.ChatRoleType.Me, Type = Core.Data.Type.ChatMessageType.String, Id = 0 });
            ChatMsgList.Add(new ChatMsgVO { Content = "测试消息", Role = Core.Data.Type.ChatRoleType.Other, Type = Core.Data.Type.ChatMessageType.String, Id = 1 });

            eventAggregator.GetEvent<SendChatMsgEvent>().Publish(newMsg);
            Context.ChatString = "";
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 更新上下文
            Context = navigationContext.Parameters.GetValue<ChatObjectVO>("context");

            // 更新历史消息队列
            ChatMsgList = chatMsgManager.GetChatMsgList(Context);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
