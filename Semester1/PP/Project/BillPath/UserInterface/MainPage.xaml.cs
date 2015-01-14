using BillPath.Models;
using BillPath.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BillPath.UserInterface
{
    public sealed partial class MainPage
        : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ExpenseGroupColorVisibilityProperty = DependencyProperty.Register("ExpenseGroupColorVisibility", typeof(Visibility), typeof(MainPage), new PropertyMetadata(Visibility.Collapsed));
        public Visibility ExpenseGroupColorVisibility
        {
            get
            {
                return (Visibility)GetValue(ExpenseGroupColorVisibilityProperty);
            }
            set
            {
                SetValue(ExpenseGroupColorVisibilityProperty, value);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ExpensesWorkspaceViewModel.Commands.UnloadAllExpensesCommand.CanExecute(null))
                ExpensesWorkspaceViewModel.Commands.UnloadAllExpensesCommand.Execute(null);
        }

        private void _IncomesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void _EditAccountsButtonClicked(object sender, RoutedEventArgs e)
        {
            _accountsMenuFlyout.Items.Clear();
            foreach (Currency availableCurrency in _expensesWorkshapeViewModel.Commands.AddAccountCommand.AvailableCurrencies)
            {
                MenuFlyoutItem menuFlyoutItem = new MenuFlyoutItem
                                                {
                                                    Text = availableCurrency.Name,
                                                    CommandParameter = availableCurrency,
                                                    Command = _expensesWorkshapeViewModel.Commands.AddAccountCommand,
                                                };
                menuFlyoutItem.Click += _HideAppBars;

                _accountsMenuFlyout.Items.Add(menuFlyoutItem);
            }
        }

        private void _HideAppBars(object sender, EventArgs e)
        {
            _bottomAppBar.IsOpen = false;
        }

        private void _CategoriesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
                foreach (CategoryViewModel newlySelectedCategory in e.AddedItems.OfType<CategoryViewModel>())
                    if (ExpensesWorkspaceViewModel.Commands.LoadExpensesCommand.CanExecute(newlySelectedCategory))
                    {
                        ExpensesWorkspaceViewModel.Commands.LoadExpensesCommand.Execute(newlySelectedCategory);

                        newlySelectedCategory.PropertyChanged += _LoadedCategoryNameChanged;
                    }

            if (e.RemovedItems != null)
                foreach (CategoryViewModel deselectedCategry in e.RemovedItems.OfType<CategoryViewModel>())
                    if (ExpensesWorkspaceViewModel.Commands.UnloadExpensesCommand.CanExecute(deselectedCategry))
                    {
                        ExpensesWorkspaceViewModel.Commands.UnloadExpensesCommand.Execute(deselectedCategry);

                        deselectedCategry.PropertyChanged -= _LoadedCategoryNameChanged;
                    }

            if (_categoriesListView.SelectedItems.Any())
            {
                _noCategoriesSelectedStackPanel.Visibility = Visibility.Collapsed;
                _detailsStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _detailsStackPanel.Visibility = Visibility.Collapsed;
                _noCategoriesSelectedStackPanel.Visibility = Visibility.Visible;
            }

            _SetLoadedCategoryNames();

            if (_categoriesListView.SelectedItems.Count == 1)
            {
                _expensesWorkshapeViewModel.Commands.RemoveCategoryCommand.Category = (CategoryViewModel)_categoriesListView.SelectedItem;
                _editCategoryButton.IsEnabled = true;
                ExpenseGroupColorVisibility = Visibility.Collapsed;
            }
            else
            {
                _expensesWorkshapeViewModel.Commands.RemoveCategoryCommand.Category = null;
                _editCategoryButton.IsEnabled = false;
                ExpenseGroupColorVisibility = Visibility.Visible;
            }
        }

        private void _LoadedCategoryNameChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Name".Equals(e.PropertyName))
                _SetLoadedCategoryNames();
        }

        private void _SetLoadedCategoryNames()
        {
            _categoryNamesTextBlock.Text = string.Join(", ", _categoriesListView.SelectedItems.OfType<CategoryViewModel>().Select(categoryViewModel => categoryViewModel.Name));
        }

        private void _OpeningEditCategoryView(object sender, object e)
        {
            _editCategoryView.DataContext = _categoriesListView.SelectedItem;
        }

        private ExpensesWorkspaceViewModel ExpensesWorkspaceViewModel
        {
            get
            {
                return _expensesWorkshapeViewModel;
            }
        }

        private void _PageDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            _expensesWorkshapeViewModel = (ExpensesWorkspaceViewModel)args.NewValue;
        }

        private void _LoadedExpensesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _loadedExpensesListView.Items.Clear();
            foreach (var item in _expensesWorkshapeViewModel.LoadedExpenses)
                _loadedExpensesListView.Items.Add(item);
        }

        private void _LoadedExpensesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _expensesWorkshapeViewModel.Commands.RemoveExpensesCommand.Expenses = _loadedExpensesListView.SelectedItems.OfType<ExpenseViewModel>();
        }

        private void _OpeningAddCategoryFlyout(object sender, object e)
        {
            CategoryViewModel categoryViewModel = (CategoryViewModel)_addCategoryView.DataContext;

            categoryViewModel.Name = string.Empty;
            categoryViewModel.ColorName = NamedColors.GetAllColorNames().First();
        }

        private void _HideAppBars(object sender, RoutedEventArgs e)
        {
            _bottomAppBar.IsOpen = false;
        }

        private void _AddExpenseFlyoutOpening(object sender, object e)
        {
            DateTime dateTimeNow = DateTime.UtcNow.ToLocalTime();

            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.DateTaken = dateTimeNow.Date;
            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.TimeTaken = new TimeSpan(dateTimeNow.TimeOfDay.Hours, dateTimeNow.TimeOfDay.Minutes, 0);

            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.Sum = 0M;
            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.Description = string.Empty;

            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.Account = _expensesWorkshapeViewModel.Accounts.First();
            _expensesWorkshapeViewModel.Commands.AddExpenseCommand.Expense.Category = ((CategoryViewModel)_categoriesListView.SelectedItem ?? _expensesWorkshapeViewModel.Categories.FirstOrDefault());
        }

        private void _AddExpenseClicked(object sender, RoutedEventArgs e)
        {
            _addExpenseFlyout.Hide();
        }

        private async void _RemoveCategoryClicked(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("You are about to delete a category and all expenses within it. This operation cannot be undone. Are you sure you want to continue?");

            messageDialog.Commands.Add(new UICommand("Cancel", (uiCommand => _expensesWorkshapeViewModel.Commands.RemoveCategoryCommand.Execute(false))));
            messageDialog.CancelCommandIndex = 0;

            messageDialog.Commands.Add(new UICommand("Delete", (uiCommand => _expensesWorkshapeViewModel.Commands.RemoveCategoryCommand.Execute(true))));
            messageDialog.DefaultCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        private void _ShowHideIncomesListViewClicked(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem showHideIncomesMenuFlyoutItem = ((MenuFlyoutItem)sender);
            ListView listView = (ListView)showHideIncomesMenuFlyoutItem.CommandParameter;

            if (listView.Visibility == Visibility.Visible)
            {
                showHideIncomesMenuFlyoutItem.Text = "Show incomes";
                listView.Visibility = Visibility.Collapsed;
            }
            else
            {
                showHideIncomesMenuFlyoutItem.Text = "Hide incomes";
                listView.Visibility = Visibility.Visible;
            }

            listView.SelectedItems.Clear();
        }

        private void _PageLoaded(object sender, RoutedEventArgs e)
        {
            _expensesWorkshapeViewModel.LoadedExpenses.CollectionChanged += _LoadedExpensesCollectionChanged;

            _expensesWorkshapeViewModel.Accounts.CollectionChanged += delegate { _EnableDisableAddIncomeButton(); _UpdateFirstTimeUseView(); };
            _expensesWorkshapeViewModel.Categories.CollectionChanged += delegate { _EnableDisableAddExpenseButton(); _UpdateFirstTimeUseView(); };

            _UpdateFirstTimeUseView();
            _EnableDisableAddIncomeButton();
            _EnableDisableAddExpenseButton();
        }

        private void _UpdateFirstTimeUseView()
        {
            if (_expensesWorkshapeViewModel.Accounts.Count == 0 && _expensesWorkshapeViewModel.Categories.Count == 0)
            {
                _contentScrollViewer.Visibility = Visibility.Collapsed;
                _firstTimeUseStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _firstTimeUseStackPanel.Visibility = Visibility.Collapsed;
                _contentScrollViewer.Visibility = Visibility.Visible;
            }
        }

        private void _EnableDisableAddIncomeButton()
        {
            _addIncomeButton.IsEnabled = (_expensesWorkshapeViewModel.Accounts.Count > 0);
        }

        private void _EnableDisableAddExpenseButton()
        {
            _addExpenseButton.IsEnabled = (_expensesWorkshapeViewModel.Categories.Count > 0);
        }

        private void _AddIncomeFlyoutOpenning(object sender, object e)
        {
            DateTime dateTimeNow = DateTime.UtcNow.ToLocalTime();
            IncomeViewModel incomeViewModel = _expensesWorkshapeViewModel.Commands.AddIncomeCommand.Income;

            incomeViewModel.Sum = 0M;
            incomeViewModel.Description = string.Empty;
            incomeViewModel.Account = _expensesWorkshapeViewModel.Accounts.FirstOrDefault();
            incomeViewModel.DateTaken = dateTimeNow.Date;
            incomeViewModel.TimeTaken = new TimeSpan(dateTimeNow.TimeOfDay.Hours, dateTimeNow.TimeOfDay.Minutes, 0);
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((AccountViewModel)((ListView)sender).DataContext).RemoveIncomesCommand.Incomes = ((ListView)sender).SelectedItems;
        }

        private ExpensesWorkspaceViewModel _expensesWorkshapeViewModel;
    }
}