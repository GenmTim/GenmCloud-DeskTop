using GenmCloud.Core.Data.Type;
using GenmCloud.Shared.Dto;
using Prism.Mvvm;

namespace GenmCloud.Core.Data.VO
{
    public class ChatMsgVO : BindableBase
    {
        // 对应服务端数据库的ID
        public uint ID { get; set; }

        private object content;
        public object Content
        {
            get => content;
            set
            {
                content = value;
                RaisePropertyChanged();
            }
        }

        public ChatRoleType Role { get; set; }

        public ChatMessageType Type { get; set; }

        private UserDto user;
        public UserDto User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged();
            }
        }
    }
}
