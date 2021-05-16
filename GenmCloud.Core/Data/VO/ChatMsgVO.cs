using GenmCloud.Core.Data.Type;
using Prism.Mvvm;
using System;

namespace GenmCloud.Core.Data.VO
{
    public class ChatMsgVO : BindableBase
    {
        // 对应服务端数据库的ID
        public long Id { get; set; }

        private string content;
        public string Content
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

        public Uri Avatar { get; set; }
    }
}
