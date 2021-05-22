using GenmCloud.ApiService.Service;
using GenmCloud.Contact.Views.SubItem;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Common;
using GenmCloud.Shared.Dto;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Contact.ViewModels
{
    public class ContactViewModel : BindableBase
    {
        private ObservableCollection<ContactDto> contactList;
        public ObservableCollection<ContactDto> ContactList
        {
            get => contactList;
            set
            {
                contactList = value;
                RaisePropertyChanged();
            }
        }

        private ContactDto selectContact;
        public ContactDto SelectContact
        {
            get => selectContact;
            set
            {
                selectContact = value;
                var param = new NavigationParameters
                {
                    { "Id", selectContact?.Id }
                };
                RegionHelper.RequestNavigate(regionManager, RegionToken.ContactInfoContent, typeof(ContactDetailView), param);
                RaisePropertyChanged();
            }
        }

        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IContactService contactService;

        public ContactViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            this.eventAggregator.GetEvent<ContactListUpdateEvent>().Subscribe(UpdateContectList);
            this.contactService = NetCoreProvider.Resolve<IContactService>();
        }

        private async void UpdateContectList()
        {
            var result = await contactService.GetAllContact();
            if (result.StatusCode == ServiceHelper.RequestOk)
            {
                if (ContactList == null)
                {
                    ContactList = new ObservableCollection<ContactDto>();
                }
                else
                {
                    ContactList.Clear();
                }

                foreach (var contact in result.Result)
                {
                    ContactList.Add(contact);
                }
            }

        }
    }
}
