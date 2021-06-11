using GenmCloud.Shared.Dto;
using Prism.Mvvm;

namespace GenmCloud.Core.Data.VO
{
    public enum ChatObjType
    {
        User,
        Group,
        Application
    }

    public class ChatObjVO : BindableBase
    {
        public uint Id { get; set; }

        private string lastMsg;
        public string LastMsg
        {
            get => lastMsg;
            set
            {
                lastMsg = value;
                RaisePropertyChanged();
            }
        }
        public long LastMsgTimestamp { get; set; }
        public ChatObjType ObjectType { get; set; }

        private int unreadMsgNumber;
        public int UnreadMsgNumber
        {
            get => unreadMsgNumber;
            set
            {
                unreadMsgNumber = value;
                RaisePropertyChanged();
            }
        }

        private string chatString;
        public string ChatString
        {
            get => chatString;
            set
            {
                chatString = value;
                RaisePropertyChanged();
            }
        }

        public UserDto Obj { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ChatObjVO other)
            {
                return other.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}
