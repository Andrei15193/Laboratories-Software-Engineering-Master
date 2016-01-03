using System;
using System.Collections.Generic;
using System.Linq;
using BillPath.DataAccess;
using BillPath.Models;
using BillPath.Modern.Common;
using BillPath.Modern.ResourceBinders;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BillPath.Modern
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Graph : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public Graph()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void _CalculateStats(object sender, RoutedEventArgs e)
        {
            try
            {
                ChartStackPanel.Visibility = Visibility.Collapsed;
                ChartProcessRing.IsActive = true;
                ChartStackPanel.Children.Clear();

                var preferredCurrency = Application.Current.GetResource<SettingsViewModel>().PreferredCurrency;
                var incomeRepository = Application.Current.GetResource<IIncomeRepository>("IIncomeRepository");
                var expenseRepository = Application.Current.GetResource<IExpenseRepository>("ExpenseRepository");

                var incomes = new List<Income>();
                var expenses = new List<Expense>();

                using (var incomeReader = await incomeRepository.GetReaderAsync())
                    while (await incomeReader.ReadAsync())
                        if (incomeReader.Current.Amount.Currency == preferredCurrency)
                            incomes.Add(incomeReader.Current);
                using (var expenseReader = await expenseRepository.GetReaderAsync(null))
                    while (await expenseReader.ReadAsync())
                        if (expenseReader.Current.Amount.Currency == preferredCurrency)
                            expenses.Add(expenseReader.Current);

                var incomesPerDays = incomes.ToLookup(income => income.DateRealized.Date);
                var expensesPerDays = expenses.ToLookup(expense => expense.DateRealized.Date);

                StartDatePicker.Date =
                    new[]
                    {
                        StartDatePicker.Date.Date,
                        incomesPerDays
                            .Select(incomesPerDay => incomesPerDay.Key)
                            .Concat(expensesPerDays
                                .Select(expensesPerDay => expensesPerDay.Key))
                            .Cast<DateTime?>()
                            .Min() ?? StartDatePicker.Date
                    }.Max().Date;
                EndDatePicker.Date =
                    new[]
                    {
                        EndDatePicker.Date.Date,
                        incomesPerDays
                            .Select(incomesPerDay => incomesPerDay.Key)
                            .Concat(expensesPerDays
                                .Select(expensesPerDay => expensesPerDay.Key))
                            .Cast<DateTime?>()
                            .Max() ?? EndDatePicker.Date
                    }.Min().Date;

                if (StartDatePicker.Date <= EndDatePicker.Date)
                {
                    var currentTotal =
                    ((from incomesPerDay in incomesPerDays
                      where incomesPerDay.Key < StartDatePicker.Date.Date
                      select incomesPerDay.Sum(income => (decimal?)income.Amount.Value))
                    .Sum() ?? 0m)
                    - ((from expensesPerDay in expensesPerDays
                        where expensesPerDay.Key < StartDatePicker.Date.Date
                        select expensesPerDay.Sum(expense => (decimal?)expense.Amount.Value))
                      .Sum() ?? 0m);

                    var maxTotal = currentTotal;
                    {
                        var currentTotalCopy = currentTotal;
                        for (var currentDate = StartDatePicker.Date.Date; currentDate <= EndDatePicker.Date.Date; currentDate = currentDate.AddDays(1))
                        {
                            currentTotalCopy = currentTotalCopy
                                + (incomesPerDays.Contains(currentDate.Date) ? incomesPerDays[currentDate.Date].Sum(income => income.Amount.Value) : 0m)
                                - (expensesPerDays.Contains(currentDate.Date) ? expensesPerDays[currentDate.Date].Sum(expense => expense.Amount.Value) : 0m);
                            if (currentTotalCopy > maxTotal)
                                maxTotal = currentTotalCopy;
                        }
                    }
                    maxTotal = Math.Abs(maxTotal);

                    if (maxTotal > 0)
                    {
                        for (var currentDate = StartDatePicker.Date.Date; currentDate <= EndDatePicker.Date.Date; currentDate = currentDate.AddDays(1))
                        {
                            currentTotal = currentTotal
                                + (incomesPerDays.Contains(currentDate.Date) ? incomesPerDays[currentDate.Date].Sum(income => income.Amount.Value) : 0m)
                                - (expensesPerDays.Contains(currentDate.Date) ? expensesPerDays[currentDate.Date].Sum(expense => expense.Amount.Value) : 0m);

                            var grid = new Grid();
                            var incomePercentage = (int)(currentTotal * 100 / maxTotal);

                            if (currentTotal < 0m)
                            {
                                grid.RowDefinitions.Add(
                                    new RowDefinition
                                    {
                                        Height = new GridLength(incomePercentage + 100, GridUnitType.Star)
                                    });
                                grid.RowDefinitions.Add(
                                    new RowDefinition
                                    {
                                        Height = new GridLength(-incomePercentage, GridUnitType.Star)
                                    });

                                var border =
                                    new Border
                                    {
                                        Child = new Rectangle
                                        {
                                            Fill = new SolidColorBrush
                                            {
                                                Color = Colors.Red
                                            }
                                        }
                                    };

                                ToolTipService.SetToolTip(border, _GetAmountText(new Amount(currentTotal, preferredCurrency)));
                                grid.Children.Add(border);
                            }
                            else
                            {
                                grid.RowDefinitions.Add(
                                    new RowDefinition
                                    {
                                        Height = new GridLength(100 - incomePercentage, GridUnitType.Star)
                                    });
                                grid.RowDefinitions.Add(
                                    new RowDefinition
                                    {
                                        Height = new GridLength(incomePercentage, GridUnitType.Star)
                                    });

                                var border =
                                    new Border
                                    {
                                        Child = new Rectangle
                                        {
                                            Fill = new SolidColorBrush
                                            {
                                                Color = Colors.Green
                                            }
                                        }
                                    };

                                ToolTipService.SetToolTip(border, _GetAmountText(new Amount(currentTotal, preferredCurrency)));
                                grid.Children.Add(border);
                            }
                            grid.Children.Add(
                                new TextBlock
                                {
                                    Text = currentDate.ToString("d")
                                });
                            ChartStackPanel.Children.Add(grid);
                        }
                    }
                }
            }
            finally
            {

                ChartProcessRing.IsActive = false;
                ChartStackPanel.Visibility = Visibility.Visible;
            }
        }

        private string _GetAmountText(Amount amount)
        {
            string currencyDisplayFormat;

            switch (Application.Current.GetResource<SettingsViewModel>().PreferredCurrencyDisplayFormat)
            {
                case CurrencyDisplayFormat.Full:
                    currencyDisplayFormat = new CurrencySynmbolAndIsoCodeFormatter().Format(amount.Currency);
                    break;

                case CurrencyDisplayFormat.Symbol:
                    currencyDisplayFormat = new CurrencySymbolOnlyFormatter().Format(amount.Currency);
                    break;

                case CurrencyDisplayFormat.IsoCode:
                    currencyDisplayFormat = new CurrencyIsoCodeOnlyFormatter().Format(amount.Currency);
                    break;

                default:
                    throw new ArgumentException(nameof(currencyDisplayFormat));
            }

            if (amount.Value < 0.01m && amount.Value >= 0m)
                return $"<{0.01m:n)}{currencyDisplayFormat}";
            else if (amount.Value > -0.01m && amount.Value < 0m)
                return $">{-0.01m:n}{currencyDisplayFormat}";
            else
                return $"{amount.Value}{currencyDisplayFormat}";
        }
    }
}