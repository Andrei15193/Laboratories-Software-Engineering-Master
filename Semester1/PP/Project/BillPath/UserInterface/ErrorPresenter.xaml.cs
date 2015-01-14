using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.UserInterface
{
    public sealed partial class ErrorPresenter
        : ItemsControl
    {
        public ErrorPresenter()
        {
            InitializeComponent();
        }

        private void _DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            _SetErrors(args.NewValue as IEnumerable);
        }

        private void _SetErrors(IEnumerable validationErrors)
        {
            if (validationErrors != null)
            {
                _rootControl.Items.Clear();

                foreach (object item in validationErrors)
                    _rootControl.Items.Add(item);
            }
        }
    }
}