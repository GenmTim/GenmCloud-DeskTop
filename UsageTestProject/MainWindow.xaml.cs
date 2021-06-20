using Prism.Regions;
using System.Windows;

namespace UsageTestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            regionManager.RegisterViewWithRegion("UploadViewContainer", typeof(UploadInfoPopup));
        }
    }
}
