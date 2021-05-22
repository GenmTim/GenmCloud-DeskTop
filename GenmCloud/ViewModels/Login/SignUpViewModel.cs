using Genm.WPF.Data.Event;
using GenmCloud.ApiService.Service;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.Dto;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

namespace GenmCloud.ViewModels.Login
{
    class SignUpViewModel : INavigationAware
    {
        private IRegionNavigationJournal journal;
        private readonly IEventAggregator eventAggregator;
        private readonly IUserService userRepository;

        public SignUpViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            this.eventAggregator = eventAggregator;
            this.userRepository = containerProvider.Resolve<IUserService>();
            eventAggregator.GetEvent<SignUpEvent>().Subscribe(SignUp);
            this.GoBackCmd = new DelegateCommand(GoBack);
        }

        public DelegateCommand GoBackCmd { get; private set; }

        async void SignUp(LoginDto loginDto)
        {
            eventAggregator.GetEvent<RunGlobalProgressEvent>().Publish(true);
            var r = await userRepository.RegisterAsync(loginDto.Username, loginDto.Password);
            eventAggregator.GetEvent<RunGlobalProgressEvent>().Publish(false);
            ToastHelper.JudgeErrorAndShow(eventAggregator, r);
            if (r != null && r.StatusCode == ServiceHelper.RequestOk)
            {
                eventAggregator.GetEvent<SignedInEvent>().Publish();
            }
        }

        private void GoBack()
        {
            journal?.GoBack();
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
            journal = navigationContext.NavigationService.Journal;
        }


    }
}
