using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BillPath.UserInterface
{
    public sealed partial class EditExpenseView
        : UserControl
    {
        public EditExpenseView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty AccountsProperty = DependencyProperty.Register("Accounts", typeof(IEnumerable), typeof(EditExpenseView), new PropertyMetadata(Enumerable.Empty<object>()));

        public IEnumerable Accounts
        {
            get
            {
                return (IEnumerable)GetValue(AccountsProperty);
            }
            set
            {
                SetValue(AccountsProperty, value);
            }
        }

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register("Categories", typeof(IEnumerable), typeof(EditExpenseView), new PropertyMetadata(Enumerable.Empty<object>()));
        public IEnumerable Categories
        {
            get
            {
                return (IEnumerable)GetValue(CategoriesProperty);
            }
            set
            {
                SetValue(CategoriesProperty, value);
            }
        }

        private void _AccountsComboBoxLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}