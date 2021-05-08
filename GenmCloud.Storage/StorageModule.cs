﻿using GenmCloud.Core.Data;
using Prism.Ioc;
using Prism.Modularity;

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

        }
    }
}