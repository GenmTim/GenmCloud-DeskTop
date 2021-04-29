using Genm.WPF.Controls;
using Genm.WPF.Data.Attributes;
using Genm.WPF.Data.Interface;
using GenmCloud.Chat.Views;
using GenmCloud.Core.Data.Token;
using GenmCloud.Core.Data.VO;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseDialogWindow, IRouteRegion
    {
        public List<MenuVO> MenuList { get; set; } = new List<MenuVO>
        {
            new MenuVO { Geometry="\xe643", Tip=new TextBlock{ Text="交流", Margin=new Thickness(3) }, UnreadMsgNumber=0, ShowFix=new Thickness(0,4,0,0), Path=RouteHelper.GetPath(typeof(ChatView))},
            new MenuVO { Geometry="\xe638", Tip=new TextBlock{ Text="文件", Margin=new Thickness(3) }, UnreadMsgNumber=0, ShowFix=new Thickness(), Path=RouteHelper.GetPath(typeof(ChatView))},
        };
        private readonly IRegionManager regionManager;

        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.menuListBox.ItemsSource = MenuList;
            Loaded += (o, s) => { RouteHelper.Route(regionManager, this.GetType(), MenuList[0].Path); };
        }

        public static string GetRouteRegionName()
        {
            return RegionToken.MainContent;
        }

        private void EliminateFocusEvent(object sender, MouseButtonEventArgs e)
        {
            this.panel.Focus();
        }
    }
}
