using HandyControl.Tools;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenmCloud.Storage.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        public StorageView()
        {
            InitializeComponent();
        }

        private Dictionary<string, TextEditor> _textEditor;
        private bool _drawerCodeUsed;

        private void DrawerCode_OnOpened(object sender, RoutedEventArgs e)
        {
            if (!_drawerCodeUsed)
            {
                var textEditorCustomStyle = ResourceHelper.GetResource<Style>("TextEditorCustom");
                _textEditor = new Dictionary<string, TextEditor>
                {
                    ["XAML"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML")
                    },
                    ["C#"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#")
                    },
                    ["VM"] = new TextEditor
                    {
                        Style = textEditorCustomStyle,
                        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#")
                    }
                };
                BorderCode.Child = new TabControl
                {
                    Style = ResourceHelper.GetResource<Style>("TabControlInLine"),
                    Items =
                    {
                        new TabItem
                        {
                            Header = "C#",
                            Content = _textEditor["C#"]
                        },
                        new TabItem
                        {
                            Header = "VM",
                            Content = _textEditor["VM"]
                        }
                    }
                };

                _drawerCodeUsed = true;
            }


            _textEditor["C#"].Text = "using HandyControl.Tools;";
            _textEditor["VM"].Text = "using System.Collections.Generic;";
        }
    }
}
