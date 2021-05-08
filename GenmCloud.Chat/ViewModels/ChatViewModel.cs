using Genm.WPF.Tools.Helper;
using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Data.VO;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Chat.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private object chatObject;
        public object ChatObject
        {
            get => chatObject;
            set
            {
                chatObject = value;
                UpdateChatBoxContext();
                RaisePropertyChanged();
            }
        }

        private readonly IRegionManager regionManager;

        public ChatViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            Message = "View A from your Prism Module";
            Simulation();
        }

        private ObservableCollection<ChatObjectVO> chatObjectVOList;
        public ObservableCollection<ChatObjectVO> ChatObjectVOList
        {
            get => chatObjectVOList;
            set
            {
                chatObjectVOList = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateChatBoxContext()
        {
            RegionHelper.RequestNavigate(regionManager, RegionToken.ChatContent, typeof(ChatBox));
        }

        public void Simulation()
        {
            ChatObjectVOList = new ObservableCollection<ChatObjectVO>();
            ChatObjectVOList.Add(new ChatObjectVO { Id = 1, LastMsg = "你好，我是蔡承龙！", LastMsgTimestamp = TimeHelper.GetNowTimeStamp(), Name = "蔡承龙", ObjectType = ChatObjectType.User, UnreadMsgNumber = 2 });
            ChatObjectVOList.Add(new ChatObjectVO { Id = 2, LastMsg = "你好，我是金泽权！Long，Long，Long，Long", LastMsgTimestamp = TimeHelper.GetNowTimeStamp(), Name = "金泽权", ObjectType = ChatObjectType.User, UnreadMsgNumber = 7 });
        }

    }
}
