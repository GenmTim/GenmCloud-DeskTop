using Genm.WPF.Controls;
using Genm.WPF.Data.Interface;
using Genm.WPF.Data.VO;
using GenmCloud.Core.Data.Token;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseDialogWindow, IRouteView
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public MainWindow(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Loaded += RegisterDefaultRegionView;
        }

        public static string GetRouteRegionName()
        {
            return RegionToken.MainContent;
        }

        private void EliminateFocusEvent(object sender, MouseButtonEventArgs e)
        {
            this.panel.Focus();
        }

        public void RegisterDefaultRegionView(object sender, RoutedEventArgs e)
        {
            if (this.menuListBox.SelectedValue is RouteMenuVO menuVo)
            {
                RouteHelper.Route(regionManager, this.GetType(), menuVo.Path);
                return;
            }
        }


    }
}
