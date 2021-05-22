using Genm.WPF.Tools.Helper;
using GenmCloud.Shared.Common.Session;
using GenmCloud.Shared.Dto;

namespace GenmCloud.Chat.Tools
{
    public static class ChatTokenGenerator
    {
        private static uint ChatToken = 0;

        public static uint GetNewToken()
        {
            return ++ChatToken;
        }
    }

    public static class ChatMsgDtoBuilder
    {
        public static ChatMsgDto BuilderStringMsg(uint otherId, string msg)
        {
            return new ChatMsgDto
            {
                Type = 1,
                SubType = 0,
                SenderId = SessionService.User.Id,
                ReceiverId = otherId,
                Timestamp = TimeHelper.GetNowTimeStamp(),
                Token = ChatTokenGenerator.GetNewToken(),
                Data = msg
            };
        }
    }

}
