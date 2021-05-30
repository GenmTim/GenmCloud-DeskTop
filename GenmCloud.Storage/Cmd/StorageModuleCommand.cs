using GenmCloud.Core.UserControls.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenmCloud.Storage.Cmd
{
    public class StorageModuleCommand

    {
        public static RoutedCommand AddNewFolderCmd { get; } = new RoutedCommand(nameof(AddNewFolderCmd), typeof(ControlCommands));
    }
}
