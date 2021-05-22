using GenmCloud.ApiService.Service;
using GenmCloud.Core.Service.Dialog;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Dto;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenmCloud.Core.UserControls.Dialog.ViewModels
{
    class QueryContactDialogViewModel : BindableBase, IDialogHostAware
    {
        public string IdentifierName { get; set; }

        public DelegateCommand SaveCmd { get; private set; }

        public DelegateCommand CancelCmd { get; private set; }

        private ObservableCollection<ContactQueryDto> queryResultList;
        public ObservableCollection<ContactQueryDto> QueryResultList
        {
            get => queryResultList;
            set
            {
                queryResultList = value;
                RaisePropertyChanged();
            }
        }

        private string queryString;
        public string QueryString
        {
            get => queryString;
            set
            {
                queryString = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand QueryUserCmd { get; private set; }

        public DelegateCommand<object> RequestContactCmd { get; private set; }

        private IContactService contactService;

        public QueryContactDialogViewModel(IContainerProvider containerProvider)
        {
            this.contactService = containerProvider.Resolve<IContactService>();
            this.QueryUserCmd = new DelegateCommand(QueryUser);

            this.CancelCmd = new DelegateCommand(() =>
            {
                DialogHost.Close(IdentifierName);
            });

            this.RequestContactCmd = new DelegateCommand<object>(RequestContact);
        }

        private async void RequestContact(object obj)
        {
            if (obj is uint id)
            {
                var result = await contactService.RequestContact(id);
                if (!ServiceHelper.IsNullOrFail(result))
                {
                    foreach (var queryRes in QueryResultList)
                    {
                        if (queryRes.Id == id)
                        {
                            queryRes.State = 1;
                        }
                    }
                }
            }
        }

        private async void QueryUser()
        {
            var result = await contactService.QueryUserToContactByEmail(QueryString);
            if (result != null && result.StatusCode == ServiceHelper.RequestOk)
            {
                var queryRes = result.Result;
                if (QueryResultList == null)
                {
                    QueryResultList = new ObservableCollection<ContactQueryDto>();
                }
                else
                {
                    QueryResultList.Clear();
                }
                QueryResultList.Add(queryRes);
            }
            else
            {
                QueryResultList = null;
            }
        }

        public System.Threading.Tasks.Task OnDialogOpenedAsync(IDialogParameters parameters)
        {
            return Task.FromResult(true);
        }
    }
}
