using BillPath.ViewModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface
{
    public sealed partial class ExpensesTotalsView
        : UserControl
    {
        public ExpensesTotalsView()
        {
            InitializeComponent();
        }

        public IValueConverter TotalsConverter
        {
            get;
            private set;
        }

        private void _DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ObservableCollection<ExpenseViewModel> expenses = args.NewValue as ObservableCollection<ExpenseViewModel>;

            if (expenses != null)
            {
                _expenses = expenses;
                expenses.CollectionChanged += delegate { SetTotals(); };
                SetTotals();
            }
        }

        private void SetTotals()
        {
            _totalsItemsPresenter.ItemsSource = _CalculateTotals();

            if (_expenses.Any())
                _viewGrid.Visibility = Visibility.Visible;
            else
                _viewGrid.Visibility = Visibility.Collapsed;
        }

        private IEnumerable _CalculateTotals()
        {
            return _expenses.GroupBy(expense => expense.Account.Currency)
                            .OrderBy(expensesByAccount => expensesByAccount.Key.Name)
                            .Select(expensesByAccount => new { Total = expensesByAccount.Sum(expenses => expenses.Sum), CurrencySymbol = expensesByAccount.Key.Symbol });
        }

        private ObservableCollection<ExpenseViewModel> _expenses;
    }
}