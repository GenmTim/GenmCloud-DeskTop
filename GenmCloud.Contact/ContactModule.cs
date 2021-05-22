using Genm.WPF.Tools.Helper;
using GenmCloud.Contact.Views;
using GenmCloud.Contact.Views.SubItem;
using GenmCloud.Core.Data;
using GenmCloud.Core.Data.Token;
using Prism.Ioc;
using Prism.Modularity;
using TMS.DeskTop.Tools.Helper;

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
            var router = Router.GetInstance();

            if (router[typeof(ContactView)] == null) throw new System.Exception("意料之外的错误");

            router[typeof(ContactDetailView)] = RouteHelper.MakeRouteInfo(typeof(ContactView), "detail", RegionToken.ContactInfoContent);
        }
    }
}