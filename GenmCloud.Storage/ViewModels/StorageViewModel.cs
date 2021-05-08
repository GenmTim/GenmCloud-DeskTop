using GenmCloud.Storage.Views;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;

namespace GenmCloud.Storage.ViewModels
{
    public class StorageViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get => isPopupOpen;
            set
            {
                isPopupOpen = value;
                RaisePropertyChanged();
            }
        }

        public StorageViewModel(IContainerProvider containerProvider)
        {
            Message = "View A from your Prism Module";
            PopupWindowCmd = new DelegateCommand(() =>
            {
                var window = new PopupWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    AllowsTransparency = true,
                    ShowTitle = false,
                    PopupElement = containerProvider.Resolve<ViewA>(),
                    MinHeight = 200,
                    MinWidth = 400,
                    WindowStyle = WindowStyle.None,
                    ShowBorder = false
                };
                window.MouseDown += (s, o) => { window.Close(); };
                window.Show();
            });
            OpenPopupCmd = new DelegateCommand(() =>
            {
                IsPopupOpen = true;
            });
        }

        public DelegateCommand PopupWindowCmd { get; private set; }

        public DelegateCommand OpenPopupCmd { get; private set; }
    }
}
