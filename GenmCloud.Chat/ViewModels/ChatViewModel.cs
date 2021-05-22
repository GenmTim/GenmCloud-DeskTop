using GenmCloud.Chat.Manager;
using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Data.VO;
using GenmCloud.Shared.Common;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Chat.ViewModels
{
    public class ChatViewModel : BindableBase, INavigationAware
    {
        private object selectedChatObject;
        public object SelectedChatObject
        {
            get => selectedChatObject;
            set
            {
                selectedChatObject = value;
                if (selectedChatObject == null) return;
                UpdateChatBoxContext(selectedChatObject);
                RaisePropertyChanged();
            }
        }

        private readonly IRegionManager regionManager;
        private readonly ChatMsgManager chatMsgManager;

        public ChatViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.chatMsgManager = NetCoreProvider.Resolve<ChatMsgManager>();
            ChatObjectVOList = chatMsgManager.GetChatObjList();
        }

        private ObservableCollection<ChatObjVO> chatObjectVOList;
        public ObservableCollection<ChatObjVO> ChatObjectVOList
        {
            get => chatObjectVOList;
            set
            {
                chatObjectVOList = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateChatBoxContext(object chatObject)
        {
            var param = new NavigationParameters
            {
                { "context", chatObject }
            };
            RegionHelper.RequestNavigate(regionManager, RegionToken.ChatContent, typeof(ChatBox), param);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var isOk = navigationContext.Parameters.TryGetValue<uint>("chatObjId", out uint chatObjId);
            if (isOk)
            {
                foreach (var chatObj in ChatObjectVOList)
                {
                    if (chatObj.Id == chatObjId)
                    {
                        SelectedChatObject = chatObj;
                        return;
                    }
                }
                chatMsgManager.AsyncGetChatObj(chatObjId);
            }
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
