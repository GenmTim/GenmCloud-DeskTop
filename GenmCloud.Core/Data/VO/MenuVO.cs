using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenmCloud.Core.Data.VO
{
    public class MenuVO : BindableBase
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
