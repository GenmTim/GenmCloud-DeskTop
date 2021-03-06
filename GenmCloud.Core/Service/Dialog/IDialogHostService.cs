using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace GenmCloud.Core.Service.Dialog
{
    /// <summary>
    /// 对话主机服务接口
    /// </summary>
    public interface IDialogHostService : IDialogService
    {
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters = null, string IdentifierName = "Root");
        //new void ShowDialog(string name, IDialogParameters parameters = null, Action<IDialogResult> callback = null);
    }
}
