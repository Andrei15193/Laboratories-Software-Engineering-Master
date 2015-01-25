using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.UserInterface
{
    public sealed partial class DisplayMoneyControl
        : UserControl
    {
        public DisplayMoneyControl()
        {
            this.InitializeComponent();
        }

        public static DependencyProperty SumProperty = DependencyProperty.Register("Sum", typeof(decimal), typeof(DisplayMoneyControl), new PropertyMetadata(default(decimal)));
        public decimal Sum
        {
            get
            {
                return (decimal)GetValue(SumProperty);
            }
            set
            {
                SetValue(SumProperty, value);
            }
        }

        public static DependencyProperty CurrencySymbolProperty = DependencyProperty.Register("CurrencySymbol", typeof(string), typeof(DisplayMoneyControl), new PropertyMetadata("<missing currency symbol>"));
        public string CurrencySymbol
        {
            get
            {
                return (string)GetValue(CurrencySymbolProperty);
            }
            set
            {
                SetValue(CurrencySymbolProperty, value);
            }
        }
    }
}