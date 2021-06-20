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
using System.Collections.Generic;
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
                var newContactList = result.Result;
                if (newContactList == null || newContactList.Count == 0) ContactList = null;

                if (ContactList == null)
                {
                    ContactList = new ObservableCollection<ContactDto>();
                }

                // 删除掉已经不存在的联系人
                Dictionary<uint, bool> set = new Dictionary<uint, bool>();
                foreach (var contact in newContactList)
                {
                    set.Add(contact.Id, true);
                }
                for (int i = ContactList.Count - 1; i >= 0; --i)
                {
                    if (!set.ContainsKey(ContactList[i].Id))
                    {
                        ContactList.Remove(ContactList[i]);
                    }
                }

                // 添加本来不存在的联系人
                var itr = result.Result.GetEnumerator();
                var index = 0;
                foreach (var contact in ContactList)
                {
                    ContactDto nowContact;
                    while (true)
                    {
                        if (false == itr.MoveNext())
                        {
                            goto loopEnd;
                        }
                        nowContact = itr.Current;
                        if (0 > string.Compare(nowContact.Name, contact.Name))
                        {
                            ContactList.Insert(index, nowContact);
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    };
                    index++;
                }
                while (itr.MoveNext() == true)
                {
                    ContactList.Add(itr.Current);
                }
            loopEnd:
                return;
            }

        }
    }
}
