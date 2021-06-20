using Genm.WPF.Tools.Helper;
using GenmCloud.Core.Data;
using GenmCloud.Core.Data.Token;
using GenmCloud.Storage.Views;
using GenmCloud.Storage.Views.ChildView;
using Prism.Ioc;
using Prism.Modularity;
using TMS.DeskTop.Tools.Helper;

namespace GenmCloud.Storage
{
    public class StorageModule : IModule
    {
        public readonly static GModuleInfo ModuleInfo = new GModuleInfo
        {
            Geometry = "\xe638",
            Name = "文件",
            Path = "",
            ShowFix = new System.Windows.Thickness(0)
        };

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var router = Router.GetInstance();

            if (router[typeof(StorageView)] == null) throw new System.Exception("意料之外的错误");

            router[typeof(HomeView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "home", RegionToken.StorageRightContent);
            router[typeof(MySpaceView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "mySpaceView", RegionToken.StorageRightContent);
            router[typeof(FavoriteView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "favoriteView", RegionToken.StorageRightContent);
            router[typeof(TrashView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "trashView", RegionToken.StorageRightContent);
            router[typeof(ShareSpaceView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "shareView", RegionToken.StorageRightContent);
            router[typeof(FolderView)] = RouteHelper.MakeRouteInfo(typeof(StorageView), "folderView", RegionToken.StorageRightContent);

        }
    }
}