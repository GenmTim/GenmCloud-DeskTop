using Prism.Mvvm;

namespace GenmCloud.Core.Data.VO
{
    public enum ChatObjectType
    {
        User,
        Group,
        Application
    }

    public class ChatObjectVO : BindableBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastMsg { get; set; }
        public long LastMsgTimestamp { get; set; }
        public ChatObjectType ObjectType { get; set; }

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
    }
}
