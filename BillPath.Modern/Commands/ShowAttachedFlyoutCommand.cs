using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace BillPath.Modern.Commands
{
    public class ShowAttachedFlyoutCommand
        : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        bool ICommand.CanExecute(object parameter)
            => true;

        public void Execute(FrameworkElement frameworkElement)
            => FlyoutBase.GetAttachedFlyout(frameworkElement).ShowAt((FrameworkElement)frameworkElement.DataContext);
        void ICommand.Execute(object parameter)
            => Execute((FrameworkElement)parameter);
    }
}