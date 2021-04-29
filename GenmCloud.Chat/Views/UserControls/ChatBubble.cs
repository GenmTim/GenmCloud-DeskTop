using Genm.WPF.Data;
using GenmCloud.Chat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.Chat.Views.UserControls
{
    public class ChatBubble : ContentControl
    {
        public static readonly DependencyProperty RoleProperty =
    DependencyProperty.Register("Role", typeof(ChatRoleType), typeof(ChatBubble), new PropertyMetadata(default(ChatRoleType)));
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
   "Type", typeof(ChatMessageType), typeof(ChatBubble), new PropertyMetadata(default(ChatMessageType)));

        public ChatRoleType Role
        {
            get { return (ChatRoleType)GetValue(RoleProperty); }
            set { SetValue(RoleProperty, value); }
        }
        public ChatMessageType Type
        {
            get => (ChatMessageType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
    }
}
