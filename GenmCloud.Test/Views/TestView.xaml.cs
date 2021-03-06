using Genm.WPF.Data.Event;
using GenmCloud.Shared.Common;
using HandyControl.Controls;
using ICSharpCode.AvalonEdit;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GenmCloud.Test.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        private ObservableCollection<DataGridColumn> columns;
        private IEventAggregator eventAggregator;

        public TestView()
        {
            InitializeComponent();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();

        }

        private Dictionary<string, TextEditor> _textEditor;
        private bool _drawerCodeUsed;

        private void DrawerCode_OnOpened(object sender, RoutedEventArgs e)
        {
            //if (!_drawerCodeUsed)
            //{
            //    var textEditorCustomStyle = ResourceHelper.GetResource<Style>("TextEditorCustom");
            //    _textEditor = new Dictionary<string, TextEditor>
            //    {
            //        ["XAML"] = new TextEditor
            //        {
            //            Style = textEditorCustomStyle,
            //            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML")
            //        },
            //        ["C#"] = new TextEditor
            //        {
            //            Style = textEditorCustomStyle,
            //            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C++")
            //        },
            //        ["VM"] = new TextEditor
            //        {
            //            Style = textEditorCustomStyle,
            //            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#")
            //        },
            //        ["Golang"] = new TextEditor
            //        {
            //            Style = textEditorCustomStyle,
            //            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Go")
            //        },
            //    };
            //    BorderCode.Child = new TabControl
            //    {
            //        Style = ResourceHelper.GetResource<Style>("TabControlInLine"),
            //        Items =
            //        {
            //            new TabItem
            //            {
            //                Header = "C#",
            //                Content = _textEditor["C#"]
            //            },
            //            new TabItem
            //            {
            //                Header = "VM",
            //                Content = _textEditor["VM"]
            //            },
            //            new TabItem
            //            {
            //                Header = "Golang",
            //                Content = _textEditor["Golang"]
            //            }
            //        }
            //    };

            //    _drawerCodeUsed = true;
            //}


            //////网络文件地址
            ////string file_url = @"http://localhost:8000/static/download.go";
            //////实例化唯一文件标识
            ////Uri file_uri = new Uri(file_url);
            //////返回文件流
            ////Stream stream = WebRequest.Create(file_uri).GetResponse().GetResponseStream();
            //////实例化文件内容
            ////StreamReader file_content = new StreamReader(stream);
            //////读取文件内容
            ////string file_content_str = file_content.ReadToEnd();

            //_textEditor["C#"].Text = "#include <iostream>";
            //_textEditor["VM"].Text = "using System.Collections.Generic;";
            //_textEditor["Golang"].Text = file_content_str;
        }

        private void SimplePanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new ImageBrowser(new Uri("http://localhost:1026/static/i1.jpg")).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eventAggregator.GetEvent<ToastShowEvent>().Publish("测试");
        }
    }
}
