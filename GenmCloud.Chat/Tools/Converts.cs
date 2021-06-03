using GenmCloud.Chat.ViewModels.UserControls.Bubble;
using GenmCloud.Chat.Views.UserControls.Bubble;
using GenmCloud.Core.Data.VO;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;

namespace GenmCloud.Chat.Tools
{
    public static class ChatObjDto2VOConvert
    {
        public static ChatObjVO Convert(ChatObjDto chatObjDto)
        {
            return new ChatObjVO
            {
                Id = chatObjDto.UserId,
                Name = chatObjDto.Nick,
                Avatar = chatObjDto.Avatar,
                LastMsg = BuilderMsgRemark(chatObjDto.LastMsg),
            };
        }

        private static string BuilderMsgRemark(ChatMsgDto chatMsgDto)
        {
            if (chatMsgDto == null) return "";
            switch (chatMsgDto.Type)
            {
                case 1:
                    return chatMsgDto.Data;
                case 2:
                    return "有新的好友请求";
                default:
                    return "";
            }
        }
    }

    public static class ChatMsgDto2VOConvert
    {
        public static ChatMsgVO Convert(ChatMsgDto chatMsgDto)
        {
            var chatMsgVO = new ChatMsgVO
            {
                Id = chatMsgDto.Id,
                Role = (chatMsgDto.SenderId == SessionService.User.ID ? Core.Data.Type.ChatRoleType.Me : Core.Data.Type.ChatRoleType.Other),
            };
            switch (chatMsgDto.Type)
            {
                case 1:
                    switch (chatMsgDto.SubType)
                    {
                        case 0:
                        case 2:
                            chatMsgVO.Content = chatMsgDto.Data;
                            chatMsgVO.Type = Core.Data.Type.ChatMessageType.String;
                            break;
                    }
                    break;
                case 2:
                    var content = NetCoreProvider.Resolve<ContactRequestBubble>();
                    if (content.DataContext is ContactRequestBubbleViewModel dataContext)
                    {
                        uint id = System.Convert.ToUInt32(chatMsgDto.Data);
                        dataContext.SetContext(id);
                    }
                    chatMsgVO.Content = content;
                    chatMsgVO.Type = Core.Data.Type.ChatMessageType.Custom;
                    break;
            }
            return chatMsgVO;
        }
    }
}
