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
            regionManager.RequestNavigate(regionName, RouteHelper.GetPath(view), param);
        }

        public static void RegisterViewWithRegion(IRegionManager regionManager, string regionName, Type viewType)
        {
            regionManager.RegisterViewWithRegion(regionName, viewType);
        }
    }
}
