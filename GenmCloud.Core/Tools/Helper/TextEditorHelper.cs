using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.IO;
using System.Windows;
using System.Xml;

namespace GenmCloud.Core.Tools.Helper
{
    public static class TextEditorHelper
    {
        public static void RegisterHighlighting(string name, string[] extensions, string filename)
        {
            XshdSyntaxDefinition xshd;
            using (Stream Write = Application.GetResourceStream(new Uri("/GenmCloud.Core;component/Resources/Styles/" + filename, UriKind.Relative)).Stream)
            {
                using (XmlTextReader reader = new XmlTextReader(Write))
                {
                    xshd = HighlightingLoader.LoadXshd(reader);
                }
            }
            if (xshd == null)
            {
                throw new Exception("意料之外的错误");
            }
            HighlightingManager.Instance.RegisterHighlighting(name, extensions, HighlightingLoader.Load(xshd, HighlightingManager.Instance));
        }
    }
}
