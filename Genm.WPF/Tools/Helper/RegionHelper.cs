using Prism.Regions;
using System;

namespace TMS.DeskTop.Tools.Helper
{
    public static class RegionHelper
    {
        public static void RequestNavigate(IRegionManager regionManager, string regionName, Type view)
        {
            RequestNavigate(regionManager, regionName, view, null);
        }

        public static void RequestNavigate(IRegionManager regionManager, string regionName, Type view, NavigationParameters param)
        {
            RequestNavigate(regionManager, regionName, RouteHelper.GetPath(view), param);
        }

        public static void RequestNavigate(IRegionManager regionManager, string regionName, string viewPath)
        {
            RequestNavigate(regionManager, regionName, viewPath, null);
        }

        public static void RequestNavigate(IRegionManager regionManager, string regionName, string viewPath, NavigationParameters param)
        {
            regionManager.RequestNavigate(regionName, viewPath, param);
        }

        public static void RegisterViewWithRegion(IRegionManager regionManager, string regionName, Type viewType)
        {
            regionManager.RegisterViewWithRegion(regionName, viewType);
        }
    }
}
