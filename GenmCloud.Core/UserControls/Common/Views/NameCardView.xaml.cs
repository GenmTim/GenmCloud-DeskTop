using Genm.WPF.Controls;
using GenmCloud.Core.Event;
using GenmCloud.Shared.Common;
using Prism.Events;
using System.Windows.Controls;

namespace GenmCloud.Core.UserControls.Common.Views
{
    /// <summary>
    /// NameCardView.xaml 的交互逻辑
    /// </summary>
    public partial class NameCardView : Popup
    {
        private readonly IEventAggregator eventAggregator;

        public NameCardView()
        {
            InitializeComponent();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<ShowNameCardEvent>().Subscribe(ShowMe);
        }

        private void ShowMe(uint id)
        {
            eventAggregator.GetEvent<UpdateNameCardContextEvent>().Publish(id);
            IsOpen = true;
        }
    }
}
