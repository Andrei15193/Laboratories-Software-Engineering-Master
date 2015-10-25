using BillPath.Modern.Converters;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Controls.Display
{
    public sealed partial class ErrorsView
        : UserControl
    {
        public ErrorsView()
        {
            InitializeComponent();
        }

        public string PropertyName
        {
            get
            {
                return (string)GetValue(PropertyNameProperty);
            }
            set
            {
                SetValue(PropertyNameProperty, value);
            }
        }
        public static DependencyProperty PropertyNameProperty { get; } =
            DependencyProperty.Register(
                nameof(PropertyName),
                typeof(string),
                typeof(ErrorsView),
                new PropertyMetadata(
                    string.Empty,
                    _PropertyNamePropertyChanged));
        private static void _PropertyNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => BindingOperations.SetBinding(
                ((ErrorsView)sender).ErrorsItemsCountrol,
                ItemsControl.ItemsSourceProperty,
                new Binding
                {
                    Path = new PropertyPath(nameof(ViewModel<object>.Errors)),
                    Converter = Application.Current.GetResource<IndexerConverter>(),
                    ConverterParameter = e.NewValue,
                    Mode = BindingMode.OneTime
                });
    }
}