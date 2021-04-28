using GenmCloud.Core.Event;
using GenmCloud.Shared.Dto;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenmCloud.ViewModels.Login
{
    class SignInViewModel
    {
        private readonly IEventAggregator eventAggregator;

        public SignInViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<SignInEvent>().Subscribe(SignIn);
        }

        public void SignIn(LoginDto loginDto)
        {
            eventAggregator.GetEvent<SignedInEvent>().Publish();
        }
    }
}
