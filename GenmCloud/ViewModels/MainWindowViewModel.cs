using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.ViewModels
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
    }

    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public List<MenuVO> MenuList { get; set; } = new List<MenuVO>
        {
            new MenuVO { Geometry="\xe643", Tip=new TextBlock{ Text="交流", Margin=new Thickness(3) }, UnreadMsgNumber=0, ShowFix=new Thickness(0,4,0,0)},
            new MenuVO { Geometry="\xe638", Tip=new TextBlock{ Text="文件", Margin=new Thickness(3) }, UnreadMsgNumber=0, ShowFix=new Thickness()},
        };

        public MainWindowViewModel()
        {

        }
    }
}
