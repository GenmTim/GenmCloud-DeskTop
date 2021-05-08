using Genm.WPF.Data.Event;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.DataInterfaces;
using GenmCloud.Shared.Dto;
using Prism.Events;
using Prism.Ioc;

namespace GenmCloud.ViewModels.Login
{
    class SignInViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IUserRepository userRepository;

        public SignInViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            this.eventAggregator = eventAggregator;
            this.userRepository = containerProvider.Resolve<IUserRepository>();
            eventAggregator.GetEvent<SignInEvent>().Subscribe(SignIn);
        }

        async void SignIn(LoginDto loginDto)
        {
            eventAggregator.GetEvent<RunGlobalProgressEvent>().Publish(true);
            var r = await userRepository.LoginAsync(loginDto.Username, loginDto.Password);
            eventAggregator.GetEvent<RunGlobalProgressEvent>().Publish(false);
            ToastHelper.JudgeErrorAndShow(eventAggregator, r);
            if (r == null && r.StatusCode ==0)
            {
                eventAggregator.GetEvent<SignedInEvent>().Publish();
            }
        }
    }
}
