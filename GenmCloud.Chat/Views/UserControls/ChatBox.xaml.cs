using HandyControl.Tools;
using System.Windows.Controls;

namespace GenmCloud.Chat.Views.UserControls
{
    /// <summary>
    /// ChatBox.xaml 的交互逻辑
    /// </summary>
    public partial class ChatBox : UserControl
    {
        private ScrollViewer _scrollViewer;

        public ChatBox()
        {
            InitializeComponent();
            listBoxChat.ItemContainerGenerator.ItemsChanged += ChatMsgListChanged;
        }

        private void ChatMsgListChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            if (_scrollViewer == null)
            {
                _scrollViewer = contentScrollViewer;
            }
            _scrollViewer?.ScrollToBottom();
        }
    }
}
