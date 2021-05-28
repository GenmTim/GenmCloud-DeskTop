using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace GenmCloud.Chat.ViewModels.UserControls.Bubble
{
    class ContactRequestBubbleViewModel : BindableBase
    {
        public DelegateCommand ShowContactRequestDetailCmd { get; set; }
        private readonly IEventAggregator eventAggregator;
        private readonly IUserService userService;


        private UserDto contact;
        public UserDto Contact
        {
            get => contact;
            set
            {
                contact = value;
                RaisePropertyChanged();
            }
        }

        public async void SetContext(uint id)
        {
            var result = await userService.GetUserInfo(id);
            if (!ServiceHelper.IsNullOrFail(result))
            {
                Contact = result.Result;
            }
        }

        public ContactRequestBubbleViewModel()
        {
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            userService = NetCoreProvider.Resolve<IUserService>();
            ShowContactRequestDetailCmd = new DelegateCommand(() =>
            {
                eventAggregator.GetEvent<ShowNameCardEvent>().Publish(Contact.Id);
            });
        }
    }
}
