using GenmCloud.ApiService.Service;
using GenmCloud.Chat.Manager;
using GenmCloud.Chat.Tools;
using GenmCloud.Core.Data.VO;
using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using static GenmCloud.Chat.Manager.ChatMsgManager;

namespace GenmCloud.Chat.ViewModels.UserControls
{
    public class ChatBoxViewModel : BindableBase, INavigationAware
    {
        private ChatObjVO context;
        public ChatObjVO Context
        {
            get => context;
            set
            {
                context = value;
                if (context == null) return;
                RaisePropertyChanged();

                eventAggregator.GetEvent<InitChatMsgListBySingleObjEvent>().Publish(context.Id);
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
        private readonly IChatService chatService;
        private readonly IEventAggregator eventAggregator;
        public DelegateCommand SendCmd { get; private set; }

        public ChatBoxViewModel()
        {
            this.chatService = NetCoreProvider.Resolve<IChatService>();
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.chatMsgManager = NetCoreProvider.Resolve<ChatMsgManager>();
            this.SendCmd = new DelegateCommand(SendMsg);
        }

        private void SendMsg()
        {
            if (string.IsNullOrEmpty(Context.ChatString))
            {
                return;
            }

            // 获取输入对象，并清空输入框
            string newMsg = Context.ChatString;
            Context.ChatString = "";

            // 构建对象，并进行事件责任的转移
            var chatMsgDto = ChatMsgDtoBuilder.BuilderStringMsg(Context.Id, newMsg);
            ChatMsgList.Add(ChatMsgDto2VOConvert.Convert(chatMsgDto));
            eventAggregator.GetEvent<SendChatMsgEvent>().Publish(chatMsgDto);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 更新上下文
            Context = navigationContext.Parameters.GetValue<ChatObjVO>("context");

            // 更新历史消息队列
            ChatMsgList = chatMsgManager.GetChatMsgListByObj(Context);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
