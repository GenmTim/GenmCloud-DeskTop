using GenmCloud.Core.UserControls.Common.Views;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listView.View = null;
        }

        private void CheckToGridView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("WrapModelViewer") as Style;
        }

        private void CheckToListView(object sender, RoutedEventArgs e)
        {
            listView.Style = listView.FindResource("GridViewModelViewer") as Style;

        }

        private void ShrinkGrid(object sender, RoutedEventArgs e)
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(220, GridUnitType.Pixel),
                To = new GridLength(0, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(1.5),
                EasingFunction = new PowerEase() { Power = 20, EasingMode = EasingMode.EaseOut }
            };
            mainGrid.ColumnDefinitions[2].BeginAnimation(ColumnDefinition.WidthProperty, d);
        }

        private void OpenGrid(object sender, RoutedEventArgs e)
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(0, GridUnitType.Pixel),
                To = new GridLength(220, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(1.5),
                EasingFunction = new PowerEase() { Power = 20, EasingMode = EasingMode.EaseOut }
            };
            mainGrid.ColumnDefinitions[2].BeginAnimation(ColumnDefinition.WidthProperty, d);
        }

        private void SimplePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (listView.View is GridView gridView)
            {
                for (int i = 0; i < Math.Min(gridView.Columns.Count, referenceGrid.ColumnDefinitions.Count); i++)
                {
                    gridView.Columns[i].Width = referenceGrid.ColumnDefinitions[i].ActualWidth;
                }
            }
        }
    }
}
