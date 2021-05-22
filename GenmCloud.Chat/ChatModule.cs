using Genm.WPF.Tools.Helper;
using GenmCloud.Chat.Views;
using GenmCloud.Chat.Views.UserControls;
using GenmCloud.Chat.Views.UserControls.Bubble;
using GenmCloud.Core.Data;
using GenmCloud.Core.Data.Token;
using Prism.Ioc;
using Prism.Modularity;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Chat
{
    public class ChatModule : IModule
    {
        public readonly static GModuleInfo ModuleInfo = new GModuleInfo
        {
            Geometry = "\xe643",
            Name = "交流",
            Path = "",
            ShowFix = new System.Windows.Thickness(0, 4, 0, 0)
        };

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var router = Router.GetInstance();

            if (router[typeof(ChatView)] == null) throw new System.Exception("意料之外的错误");

            router[typeof(ChatBox)] = RouteHelper.MakeRouteInfo(typeof(ChatView), "chatbox", RegionToken.ChatContent);

            containerRegistry.Register<ContactRequestBubble>();
        }
    }
}