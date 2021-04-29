using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Core.Data.Token;
using Prism.Mvvm;
using Prism.Regions;
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
        }


        public void UpdateChatBoxContext()
        {
            RegionHelper.RequestNavigate(regionManager, RegionToken.ChatContent, typeof(ChatBox));
        }

    }
}
