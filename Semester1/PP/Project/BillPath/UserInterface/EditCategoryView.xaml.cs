using BillPath.ViewModels;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.UserInterface
{
    public sealed partial class EditCategoryView
        : UserControl
    {
        public EditCategoryView()
        {
            InitializeComponent();
        }

        private void _ColorComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _CategoryViewModel.ColorName = ((NamedColor)_colorComboBox.SelectedItem).Name;
        }

        private void _ColorComboBoxLoaded(object sender, RoutedEventArgs e)
        {
        }

        private CategoryViewModel _CategoryViewModel
        {
            get
            {
                return (CategoryViewModel)DataContext;
            }
        }

        private void _DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (_CategoryViewModel != null)
                if (!string.IsNullOrWhiteSpace(_CategoryViewModel.ColorName))
                    _colorComboBox.SelectedIndex = NamedColors.GetAllNamedColors().TakeWhile(namedColor => !namedColor.Name.Equals(_CategoryViewModel.ColorName)).Count();
        }
    }
}