using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genm.WPF.Tools.Helper
{
    public class Router
    {
        private bool isInjection = false;
        private static Router instance = null;
        public static Router Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Router();
                }
                return instance;
            }
        }

        private Dictionary<Type, string> routeMap;
        public Dictionary<Type, string> RouteMap
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
        public Dictionary<string, Type> RouteReMap
        {
            get
            {
                if (!isInjection)
                {
                    throw new Exception("You Need Injection!!!");
                }
                return routeReMap;
            }
        }

        public Dictionary<Type, string> RegionNameMap { get; set; }

        public void RegisterRegionName(Type view, string regionToken)
        {
            RegionNameMap.Add(view, regionToken);
        }

        public void Injection()
        {
            routeReMap = new Dictionary<string, Type>();
            foreach (KeyValuePair<Type, string> kvp in RouteMap)
            {
                routeReMap.Add(kvp.Value, kvp.Key);
            }
            isInjection = true;
        }

        public string GetPath(Type view)
        {
            if (!RouteMap.ContainsKey(view))
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

        public string GetRegionName(Type view)
        {
            if (!RegionNameMap.ContainsKey(view))
            {
                throw new Exception("无注册的区域");
            }
            return RegionNameMap[view];
        }
    }
}
