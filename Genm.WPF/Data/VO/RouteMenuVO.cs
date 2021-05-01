using Prism.Mvvm;
using System.Windows;

namespace Genm.WPF.Data.VO
{
    public class RouteMenuVO
        : BindableBase
    {
        public string Geometry { get; set; }

        private object tip;
        public object Tip
        {
            get => tip;
            set
            {
                tip = value;
                RaisePropertyChanged();
            }
        }

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

        public Thickness ShowFix { get; set; }

        public string Path { get; set; }
    }
}
