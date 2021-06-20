using GenmCloud.Core.Service.Dialog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace GenmCloud.Storage.ViewModels.Dlg
{
    class AddFolderDlgViewModel : BindableBase, IDialogHostAware
    {
        public string IdentifierName { get; set; }

        public DelegateCommand SaveCmd { get; private set; }

        public DelegateCommand CancelCmd { get; private set; }

        public Task OnDialogOpenedAsync(IDialogParameters parameters)
        {
            return Task.FromResult(true);
        }
    }
}
