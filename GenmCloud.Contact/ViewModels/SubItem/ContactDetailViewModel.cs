using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace GenmCloud.Contact.ViewModels.SubItem
{
    class ContactDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IUserService userService;

        private UserDto context;
        public UserDto Context
        {
            get => context;
            set
            {
                context = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand GoChatCmd { get; private set; }

        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public ContactDetailViewModel(IRegionManager regionManager)
        {
            this.userService = NetCoreProvider.Resolve<IUserService>();
            this.eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            this.regionManager = regionManager;
            this.GoChatCmd = new DelegateCommand(() =>
            {
                eventAggregator.GetEvent<GoChatEvent>().Publish(Context.Id);
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = navigationContext.Parameters.GetValue<uint>("Id");
            UpdateContext(id);
        }

        private async void UpdateContext(uint id)
        {
            var result = await userService.GetUserInfo(id);
            if (result.StatusCode == ServiceHelper.RequestOk)
            {
                Context = result.Result;
            }
        }
    }
}
