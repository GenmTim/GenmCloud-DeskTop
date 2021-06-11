using Genm.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenmCloud.Core.UserControls.Popups
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ProfilePopup : Popup
    {
        public ProfilePopup()
        {
            InitializeComponent();
        }

        private void ChangeAvatar(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                InitialDirectory = @"C:\Users\pc\Desktop",//设置文件打开初始目录为桌面
                Title = "请选择图片",//设置打开文件对话框标题
                Filter = "图片文件(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp;*.png",//设置文件过滤类型
                RestoreDirectory = true //设置对话框是否记忆之前打开的目录
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//当点击文件对话框的确定按钮时打开相应的文件
            {
                //将选中的文件的路径传递给相应控件比如Image，PictureBox
            }
            e.Handled = true;
        }
    }
}
