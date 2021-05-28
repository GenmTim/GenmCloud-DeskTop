using GenmCloud.Core.Data;
using Prism.Ioc;
using Prism.Modularity;

namespace GenmCloud.Test
{
    public class TestModule : IModule
    {
        public readonly static GModuleInfo ModuleInfo = new GModuleInfo
        {
            Geometry = "\xe644",
            Name = "测试",
            Path = "",
            ShowFix = new System.Windows.Thickness(0)
        };

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}