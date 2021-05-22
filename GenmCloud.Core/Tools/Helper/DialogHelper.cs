using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.UserControls.Dialog.Views;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace GenmCloud.Core.Tools.Helper
{
    public static class DialogHelper
    {
        public static Task<IDialogResult> ShowInputValueDialog(IDialogHostService dialogHostService, string identifierName, string title, string positiveText, string negativeText, string inputHint = "")
        {
            var param = new DialogParameters();
            param.Add("title", title);
            param.Add("positive_text", positiveText);
            param.Add("negative_text", negativeText);
            param.Add("input_hint", inputHint);
            return dialogHostService.ShowDialog(nameof(InputValueDialog), param, identifierName);
        }

        public static Task<IDialogResult> ShowQuestionDialog(IDialogHostService dialogHostService, string identifierName, string title, string positiveText, string negativeText, string question = "")
        {
            var param = new DialogParameters();
            param.Add("title", title);
            param.Add("positive_text", positiveText);
            param.Add("negative_text", negativeText);
            param.Add("question", question);
            return dialogHostService.ShowDialog(nameof(QuestionDialog), param, identifierName);
        }
    }
}
