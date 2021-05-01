using Prism.Ioc;
using System;
using System.Collections.Generic;

namespace Genm.WPF.Tools.Helper
{
    public class RouteInfo
    {
        public string Path { get; set; }
        public string ParentRegion { get; set; }
    }

    public class Router
    {
        private IContainerRegistry containerRegistry;

        private Router(IContainerRegistry containerRegistry)
        {
            this.containerRegistry = containerRegistry;
        }

        private static Router instance;

        public static Router Instance(IContainerRegistry containerRegistry)
        {
            if (instance == null)
            {
                instance = new Router(containerRegistry);
            }
            return instance;
        }

        public static Router GetInstance()
        {
            if (instance == null)
            {
                throw new Exception("需要使用Instance完成构造，才能进行调用");
            }
            return instance;
        }

        private Dictionary<Type, string> routeMap;
        private Dictionary<Type, string> RouteMap
        {
            get
            {
                if (routeMap == null)
                {
                    routeMap = new Dictionary<Type, string>();
                }
                return routeMap;
            }
        }

        private Dictionary<string, Type> routeReMap;
        private Dictionary<string, Type> RouteReMap
        {
            get
            {
                if (routeReMap == null)
                {
                    routeReMap = new Dictionary<string, Type>();
                }
                return routeReMap;
            }
        }

        private Dictionary<Type, string> regionMap;
        private Dictionary<Type, string> RegionMap
        {
            get
            {
                if (regionMap == null)
                {
                    regionMap = new Dictionary<Type, string>();
                }
                return regionMap;
            }
        }

        public RouteInfo this[Type view]
        {
            get
            {
                var routeInfo = new RouteInfo();
                if (HasRegisteredViewPath(view)) routeInfo.Path = GetPath(view);
                if (HasRegisteredViewParentRegion(view)) routeInfo.ParentRegion = GetParentRegion(view);
                return routeInfo;
            }
            set
            {
                RegisterRouteInfo(view, value);
            }
        }

        public void RegisterRouteInfo(Type view, RouteInfo info)
        {
            RegisterPath(view, info.Path);
            RegisterParentRegion(view, info.ParentRegion);
        }

        private void RegisterPath(Type view, string path)
        {
            RouteMap.Add(view, path);
            RouteReMap.Add(path, view);
            containerRegistry.RegisterForNavigation(view, path);
        }

        private void RegisterParentRegion(Type view, string regionToken)
        {
            if (string.IsNullOrEmpty(regionToken)) return;
            RegionMap.Add(view, regionToken);
        }

        public string GetPath(Type view)
        {
            if (!HasRegisteredViewPath(view))
            {
                throw new Exception("无注册的视图");
            }
            return RouteMap[view];
        }

        public Type GetView(string path)
        {
            if (!RouteReMap.ContainsKey(path))
            {
                throw new Exception("此路径，无对应的注册视图");
            }
            return RouteReMap[path];
        }

        public string GetParentRegion(Type view)
        {
            if (!HasRegisteredViewParentRegion(view))
            {
                throw new Exception("无注册的区域");
            }
            return RegionMap[view];
        }

        public bool HasRegisteredViewPath(Type view)
        {
            return RouteMap.ContainsKey(view);
        }

        public bool HasRegisteredViewParentRegion(Type view)
        {
            return RegionMap.ContainsKey(view);
        }
    }
}
