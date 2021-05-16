using GenmCloud.Contact.Views;
using GenmCloud.Core.Data;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GenmCloud.Contact
{
    public class ContactModule : IModule
    {
        public readonly static GModuleInfo ModuleInfo = new GModuleInfo
        {
            Geometry = "\xe648",
            Name = "联系",
            Path = "",
            ShowFix = new System.Windows.Thickness(0, 0, 0, 0)
        };

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}