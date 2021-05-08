namespace GenmCloud.Shared.Common
{
    using Prism.Ioc;
    using System;

    /// <summary>
    /// 服务提供者
    /// </summary>
    public class NetCoreProvider
    {
        public static IContainerProvider Instance { get; private set; }

        public static void RegisterServiceLocator(IContainerProvider containerProvider)
        {
            if (Instance == null)
                Instance = containerProvider;
        }

        public static T Resolve<T>()
        {
            if (!Instance.IsRegistered<T>())
                new ArgumentNullException(nameof(T));

            return Instance.Resolve<T>();
        }

        public static T ResolveNamed<T>(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                new ArgumentNullException(nameof(T));

            return Instance.Resolve<T>(typeName);
        }
    }
}
