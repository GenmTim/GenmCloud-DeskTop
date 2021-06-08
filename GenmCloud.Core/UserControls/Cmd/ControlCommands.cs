using System.Windows.Input;

namespace GenmCloud.Core.UserControls.Cmd
{
    public static class ControlCommands
    {
        public static RoutedCommand CompleteCheckCmd { get; } = new RoutedCommand(nameof(CompleteCheckCmd), typeof(ControlCommands));

        public static RoutedCommand AddNewFolderCmd { get; } = new RoutedCommand(nameof(AddNewFolderCmd), typeof(ControlCommands));
    }
}
