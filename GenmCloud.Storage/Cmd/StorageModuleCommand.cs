using GenmCloud.Core.UserControls.Cmd;
using System.Windows.Input;

namespace GenmCloud.Storage.Cmd
{
    public class StorageModuleCommand

    {
        public static RoutedCommand AddNewFolderCmd { get; } = new RoutedCommand(nameof(AddNewFolderCmd), typeof(ControlCommands));
    }
}
