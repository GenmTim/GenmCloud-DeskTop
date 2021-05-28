using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace GenmCloud.Core.UserControls.Common.ViewModels
{
    class NameCardViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        private UserDto user;
        public UserDto User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged();
            }
        }


        private int contactState;
        public int ContactState
        {
            get => contactState;
            set
            {
                contactState = value;
                RaisePropertyChanged();
            }
        }

        private bool btnIsEnable = true;
        public bool BtnIsEnable
        {
            get => btnIsEnable;
            set
            {
                btnIsEnable = value;
                RaisePropertyChanged();
            }
        }

        private readonly IUserService userService;
        private readonly IContactService contactService;

        public DelegateCommand AssentContactRequestCmd { get; private set; }
        public DelegateCommand RequestContactCmd { get; private set; }
        //public DelegateCommand GoChatCmd { get; private set; }

        public NameCardViewModel()
        {
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<UpdateNameCardContextEvent>().Subscribe(UpdateContext);
            userService = NetCoreProvider.Resolve<IUserService>();
            contactService = NetCoreProvider.Resolve<IContactService>();
            AssentContactRequestCmd = new DelegateCommand(AssentContactRequest);
            RequestContactCmd = new DelegateCommand(RequestContact);
        }

        private async void AssentContactRequest()
        {
            BtnIsEnable = false;
            var result = await contactService.AssentRequestContact(User.Id);
            if (!ServiceHelper.IsNullOrFail(result))
            {
                ContactState = 2;
            }
            BtnIsEnable = true;
        }

        private async void RequestContact()
        {
            BtnIsEnable = false;
            var result = await contactService.RequestContact(User.Id);
            if (!ServiceHelper.IsNullOrFail(result))
            {
                ContactState = 1;
            }
            BtnIsEnable = true;
        }

        private async void UpdateContext(uint id)
        {
            var userRes = await userService.GetUserInfo(id);
            if (!ServiceHelper.IsNullOrFail(userRes))
            {
                User = userRes.Result;
            }

            var stateRes = await contactService.QueryUserContactStateById(id);
            if (!ServiceHelper.IsNullOrFail(stateRes))
            {
                var state = stateRes.Result;
                ContactState = state.State;
            }
        }
    }
}
