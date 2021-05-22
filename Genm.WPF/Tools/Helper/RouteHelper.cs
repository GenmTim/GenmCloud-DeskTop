using Genm.WPF.Tools.Helper;
using Prism.Regions;
using System;

namespace TMS.DeskTop.Tools.Helper
{
    public static class RouteHelper
    {
        public static string RouteTag = "_rh_target_view";

        public static void Goto(IRegionManager regionManager, Type nowView, Type targetView)
        {
            Goto(regionManager, nowView, targetView, null);
        }

        public static void Goto(IRegionManager regionManager, Type nowView, string targetName)
        {
            Goto(regionManager, nowView, GetView(targetName), null);
        }

        public static void Goto(IRegionManager regionManager, Type nowView, Type targetView, NavigationParameters param)
        {
            // 寻找公共视图
            Type common_view = FindCommonView(nowView, targetView);

            // 寻找公共区域的注入视图，以进行下一步导航
            string next_route = GetNextRoute(GetPath(common_view), GetPath(targetView));

            // 发送导航请求
            if (!next_route.Equals(""))
            {
                if (param == null)
                {
                    param = new NavigationParameters();
                }
                param.Add(RouteTag, targetView);

                RegionHelper.RequestNavigate(regionManager, GetParentRegionName(GetView(next_route)), next_route, param);
            }
        }

        public static RouteInfo MakeRouteInfo(string path)
        {
            return new RouteInfo { Path = path };
        }

        public static RouteInfo MakeRouteInfo(Type parentView, string subPath, string parentRegion)
        {
            return new RouteInfo { Path = GetPath(parentView) + subPath, ParentRegion = parentRegion };
        }

        /// <summary>
        /// 实现直线路由
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="nowView">现在所在的视图的名字</param>
        /// <param name="targetView">目标视图的名字</param>
        public static void Route(IRegionManager regionManager, Type nowView, Type targetView)
        {
            // 寻找下个路由
            string next_route = GetNextRoute(GetPath(nowView), GetPath(targetView));

            // 发送导航请求
            if (!next_route.Equals(""))
            {
                NavigationParameters param = new NavigationParameters();
                param.Add(RouteTag, targetView);

                RegionHelper.RequestNavigate(regionManager, GetParentRegionName(nowView), next_route, param);
            }
        }

        /// <summary>
        /// 实现直线路由
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="nowView">现在所在的视图的名字</param>
        /// <param name="targetView">目标视图的名字</param>
        public static void Route(IRegionManager regionManager, Type nowView, string targetViewPath)
        {
            // 寻找下个路由
            string next_route = GetNextRoute(GetPath(nowView), targetViewPath);

            // 发送导航请求
            if (!next_route.Equals(""))
            {
                NavigationParameters param = new NavigationParameters();
                param.Add(RouteTag, GetView(targetViewPath));

                RegionHelper.RequestNavigate(regionManager, GetParentRegionName(GetView(next_route)), next_route, param);
            }
        }

        public static string GetPath(Type view)
        {
            return Router.GetInstance().GetPath(view);
        }

        public static Type GetView(string path)
        {
            return Router.GetInstance().GetView(path);
        }

        public static string GetParentRegionName(Type view)
        {
            return Router.GetInstance().GetParentRegion(view);
        }

        /// <summary>
        ///  寻找公共视图
        /// </summary>
        /// <param name="view1"></param>
        /// <param name="view2"></param>
        /// <returns></returns>
        private static Type FindCommonView(Type view1, Type view2)
        {
            // 取出目前路径
            string view1Path = GetPath(view1);

            // 取出目标路径
            string view2Path = GetPath(view2);

            int index = -1;
            for (int i = 0; i < Math.Min(view1Path.Length, view2Path.Length); ++i)
            {
                if (view1Path[i] != view2Path[i]) break;

                if (view1Path[i] == '/')
                {
                    index = i;
                }
            }

            string common_url;
            if (index == -1)
            {
                throw new Exception("错误的路由");
            }
            common_url = view1Path.Substring(0, index + 1);
            return GetView(common_url);
        }

        /// <summary>
        /// 获取下一个视图地址
        /// </summary>
        /// <param name="nowUrl">现在view的url路径</param>
        /// <param name="targetUrl">目标view的url路径</param>
        /// <returns></returns>
        public static string GetNextRoute(string nowUrl, string targetUrl)
        {
            int next_index = targetUrl.IndexOf('/', nowUrl.Length);
            string next_route = targetUrl.Substring(0, next_index + 1);
            return next_route;
        }
    }
}
