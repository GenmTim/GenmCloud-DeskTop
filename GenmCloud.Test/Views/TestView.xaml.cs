using ICSharpCode.AvalonEdit;
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

        public TestView()
        {
            InitializeComponent();

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



    }
}
