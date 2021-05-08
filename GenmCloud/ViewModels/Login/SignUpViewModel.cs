using Genm.WPF.Data.Event;
using GenmCloud.Core.Event;
using GenmCloud.Core.Tools.Helper;
using GenmCloud.Shared.DataInterfaces;
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
        private readonly IUserRepository userRepository;

        public SignUpViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            this.eventAggregator = eventAggregator;
            this.userRepository = containerProvider.Resolve<IUserRepository>();
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
            if (r != null && r.StatusCode == 0)
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
