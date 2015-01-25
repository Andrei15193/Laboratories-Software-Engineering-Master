using BillPath.DataAccess;
using BillPath.Models;
using BillPath.ViewModels.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BillPath.ViewModels
{
    internal class ExpensesWorkspaceViewModel
    {
        internal ExpensesWorkspaceViewModel()
        {
            _appData = new Lazy<AppData>(() => AppDataProvider.AppData);
            _accounts = new Lazy<ObservableCollection<AccountViewModel>>(() => new ObservableCollection<AccountViewModel>(_appData.Value.Accounts.Select(account => new AccountViewModel(account, this))));
            _categories = new Lazy<ObservableCollection<CategoryViewModel>>(() => new ObservableCollection<CategoryViewModel>(_appData.Value.Categories.Select(category => new CategoryViewModel(category, this))));

            LoadedCategories = new ObservableCollection<CategoryViewModel>();
            LoadedExpenses = new ObservableCollection<ExpenseViewModel>();

            Commands = new ExpensesWorkspaceCommands(this);
        }

        public IAppDataProvider AppDataProvider
        {
            get;
            set;
        }

        public ObservableCollection<AccountViewModel> Accounts
        {
            get
            {
                return _accounts.Value;
            }
        }

        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                return _categories.Value;
            }
        }

        public ObservableCollection<CategoryViewModel> LoadedCategories
        {
            get;
            private set;
        }
        public ObservableCollection<ExpenseViewModel> LoadedExpenses
        {
            get;
            private set;
        }

        public ExpensesWorkspaceCommands Commands
        {
            get;
            private set;
        }

        internal CategoryViewModel GetViewModelForCategory(Category category)
        {
            return Categories.FirstOrDefault(categoryViewModel => categoryViewModel.Model == category);
        }

        internal AccountViewModel GetViewModelForAccount(Account account)
        {
            if (account == null)
                return null;
            else
                return Accounts.First(accountViewModel => accountViewModel.Model == account);
        }

        internal AppData AppData
        {
            get
            {
                return _appData.Value;
            }
        }

        private readonly Lazy<AppData> _appData;
        private readonly Lazy<ObservableCollection<AccountViewModel>> _accounts;
        private readonly Lazy<ObservableCollection<CategoryViewModel>> _categories;

        public sealed class ExpensesWorkspaceCommands
        {
            internal sealed class AddAccount
                : Command<ExpensesWorkspaceViewModel, Currency>
            {
                internal AddAccount(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                }

                public IEnumerable<Currency> AvailableCurrencies
                {
                    get
                    {
                        return Currencies.AllCurrencies.Except(ViewModel.Accounts.Select(account => account.Currency)).OrderBy(currency => currency.Name);
                    }
                }

                public override bool CanExecute(Currency currency)
                {
                    return (currency != null && ViewModel.Accounts.All(account => account.Currency != currency));
                }

                public override void Execute(Currency currency)
                {
                    Account newAccount = new Account
                                         {
                                             CurrencyName = currency.Name
                                         };
                    ViewModel.AppData.Accounts.Add(newAccount);

                    ViewModel.Accounts.Add(new AccountViewModel(newAccount, ViewModel));
                }
            }
            internal sealed class AddIncome
                : Command<ExpensesWorkspaceViewModel>
            {
                internal AddIncome(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                    Income = new IncomeViewModel(new Income
                                                 {
                                                     Account = null,
                                                     DateTaken = DateTime.UtcNow.ToLocalTime(),
                                                     Description = string.Empty,
                                                     Sum = 0M
                                                 },
                                                 viewModel);
                    Income.PropertyChanged += delegate { OnCanExecuteChanged(); };
                }

                public IncomeViewModel Income
                {
                    get;
                    private set;
                }

                public override bool CanExecute(object parameter)
                {
                    return !Income.ValidationErrors.OfType<ValidationError>().Any();
                }

                public override void Execute(object parameter)
                {
                    Income newIncome = new Income
                                       {
                                           Account = Income.Account.Model,
                                           DateTaken = Income.DateTimeTaken,
                                           Description = Income.Description,
                                           Sum = Income.Sum
                                       };
                    IncomeViewModel newIncomeViewModel = new IncomeViewModel(newIncome, ViewModel);

                    _Insert(newIncome, newIncomeViewModel);
                }

                private void _Insert(Income newIncome, IncomeViewModel newIncomeViewModel)
                {
                    int insertIndex = Income.Account.Incomes.TakeWhile(incomeViewModel => incomeViewModel.DateTimeTaken.CompareTo(newIncomeViewModel.DateTimeTaken) >= 0).Count();
                    Income.Account.Incomes.Insert(insertIndex, newIncomeViewModel);
                    Income.Account.Model.Incomes.Insert(insertIndex, newIncome);
                }
            }

            internal sealed class AddExpense
                : Command<ExpensesWorkspaceViewModel>
            {
                internal AddExpense(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                    Expense = new ExpenseViewModel(new Expense
                                                   {
                                                       Account = null,
                                                       Category = null,
                                                       DateTaken = DateTime.UtcNow.ToLocalTime(),
                                                       Description = string.Empty,
                                                       Sum = 0M
                                                   },
                                                   viewModel);
                    Expense.PropertyChanged += delegate { OnCanExecuteChanged(); };
                }

                public ExpenseViewModel Expense
                {
                    get;
                    private set;
                }

                public override bool CanExecute(object parameter)
                {
                    return !Expense.ValidationErrors.OfType<ValidationError>().Any();
                }

                public override void Execute(object parameter)
                {
                    Expense newExpense = new Expense
                                         {
                                             Account = Expense.Account.Model,
                                             Category = Expense.Category.Model,
                                             DateTaken = Expense.DateTimeTaken,
                                             Description = Expense.Description,
                                             Sum = Expense.Sum
                                         };
                    ExpenseViewModel newExpenseViewModel = new ExpenseViewModel(newExpense, ViewModel);

                    _Insert(newExpense, newExpenseViewModel);
                }

                private void _Insert(Expense newExpense, ExpenseViewModel newExpenseViewModel)
                {
                    int insertIndex = Expense.Category.Expenses.TakeWhile(expenseViewModel => ViewModel._expenseComparer.Compare(expenseViewModel, newExpenseViewModel) <= 0).Count();
                    Expense.Category.Expenses.Insert(insertIndex, newExpenseViewModel);
                    Expense.Category.Model.Expenses.Insert(insertIndex, newExpense);

                    if (ViewModel.LoadedCategories.Contains(Expense.Category))
                        ViewModel.LoadedExpenses.Insert(ViewModel.LoadedExpenses.TakeWhile(expenseViewModel => ViewModel._expenseComparer.Compare(expenseViewModel, newExpenseViewModel) <= 0).Count(), newExpenseViewModel);
                }
            }
            internal sealed class RemoveExpenses
                : Command<ExpensesWorkspaceViewModel>
            {
                internal RemoveExpenses(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                    _expenses = Enumerable.Empty<ExpenseViewModel>();
                }

                public IEnumerable<ExpenseViewModel> Expenses
                {
                    get
                    {
                        return _expenses;
                    }
                    set
                    {
                        _expenses = (value ?? Enumerable.Empty<ExpenseViewModel>());
                        OnCanExecuteChanged();
                    }
                }

                public override bool CanExecute(object parameter)
                {
                    return _expenses.Any();
                }

                public override void Execute(object parameter)
                {
                    foreach (ExpenseViewModel expense in _expenses.ToList())
                    {
                        ViewModel.LoadedExpenses.Remove(expense);
                        expense.Category.Expenses.Remove(expense);
                        expense.Category.Model.Expenses.Remove(expense.Model);
                    }
                }

                private IEnumerable<ExpenseViewModel> _expenses;
            }

            internal sealed class AddCategory
                : Command<ExpensesWorkspaceViewModel>
            {
                internal AddCategory(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                    Category = new CategoryViewModel(new Category { Name = string.Empty, ColorName = NamedColors.GetAllColorNames().First() });
                    Category.PropertyChanged += delegate { OnCanExecuteChanged(); };
                }

                public CategoryViewModel Category
                {
                    get;
                    private set;
                }

                public override bool CanExecute(object parameter)
                {
                    return (Category != null && !Category.ValidationErrors.OfType<ValidationError>().Any());
                }

                public override void Execute(object parameter)
                {
                    Category newCategory = new Category
                                           {
                                               Name = Category.Name,
                                               ColorName = Category.ColorName
                                           };

                    ViewModel.Categories.Add(new CategoryViewModel(newCategory, ViewModel));
                    ViewModel.AppData.Categories.Add(newCategory);
                }
            }
            internal sealed class RemoveCategory
                : Command<ExpensesWorkspaceViewModel>
            {
                internal RemoveCategory(ExpensesWorkspaceViewModel viewModel)
                    : base(viewModel)
                {
                }

                public CategoryViewModel Category
                {
                    get
                    {
                        return _category;
                    }
                    set
                    {
                        _category = value;
                        OnCanExecuteChanged();
                    }
                }

                public override bool CanExecute(object parameter)
                {
                    return (Category != null);
                }

                public override void Execute(object parameter)
                {
                    if (parameter is bool && (bool)parameter)
                    {
                        foreach (ExpenseViewModel expenseViewModel in Category.Expenses)
                            ViewModel.LoadedExpenses.Remove(expenseViewModel);

                        ViewModel.AppData.Categories.Remove(Category.Model);
                        ViewModel.Categories.Remove(Category);
                    }
                }

                private CategoryViewModel _category;
            }

            internal ExpensesWorkspaceCommands(ExpensesWorkspaceViewModel viewModel)
            {
                if (viewModel == null)
                    throw new ArgumentNullException("viewModel");

                _viewModel = viewModel;

                LoadExpensesCommand = new RelayCommand<CategoryViewModel>(_LoadExpenses);
                UnloadExpensesCommand = new RelayCommand<CategoryViewModel>(_UnloadExpenses);
                UnloadAllExpensesCommand = new RelayCommand(_UnloadAllExpensesCommand);

                AddAccountCommand = new AddAccount(viewModel);
                AddIncomeCommand = new AddIncome(viewModel);

                AddCategoryCommand = new AddCategory(viewModel);
                RemoveCategoryCommand = new RemoveCategory(viewModel);

                AddExpenseCommand = new AddExpense(viewModel);
                RemoveExpensesCommand = new RemoveExpenses(viewModel);
            }

            public ICommand LoadExpensesCommand
            {
                get;
                private set;
            }

            public ICommand UnloadExpensesCommand
            {
                get;
                private set;
            }

            public ICommand UnloadAllExpensesCommand
            {
                get;
                private set;
            }

            public AddAccount AddAccountCommand
            {
                get;
                private set;
            }
            public AddIncome AddIncomeCommand
            {
                get;
                private set;
            }

            public AddExpense AddExpenseCommand
            {
                get;
                private set;
            }
            public RemoveExpenses RemoveExpensesCommand
            {
                get;
                private set;
            }

            public AddCategory AddCategoryCommand
            {
                get;
                private set;
            }
            public RemoveCategory RemoveCategoryCommand
            {
                get;
                private set;
            }

            private void _LoadExpenses(CategoryViewModel category)
            {
                int startIndex = 0;

                _viewModel.LoadedCategories.Add(category);
                foreach (ExpenseViewModel expenseViewModel in category.Expenses.OrderBy(expense => expense, _viewModel._expenseComparer))
                    startIndex = _Load(expenseViewModel, startIndex);
            }

            private int _Load(ExpenseViewModel expense, int startIndex)
            {
                int insertIndex = startIndex;

                if (_viewModel.LoadedExpenses.Any())
                {
                    while (insertIndex < _viewModel.LoadedExpenses.Count && _viewModel._expenseComparer.Compare(expense, _viewModel.LoadedExpenses[insertIndex]) >= 0)
                        insertIndex++;

                    _viewModel.LoadedExpenses.Insert(insertIndex, expense);

                    return insertIndex;
                }
                else
                {
                    _viewModel.LoadedExpenses.Add(expense);
                    return 0;
                }
            }

            private void _UnloadExpenses(CategoryViewModel category)
            {
                _viewModel.LoadedCategories.Remove(category);
                foreach (int expenseIndexToRemove in _viewModel.LoadedExpenses
                                                               .Select((expense, expenseIndex) => new { Category = expense.Category, ExpenseIndex = expenseIndex })
                                                               .Where(expenseInfo => category == expenseInfo.Category)
                                                               .Select(expenseInfo => expenseInfo.ExpenseIndex)
                                                               .Reverse()
                                                               .ToList())
                    _viewModel.LoadedExpenses.RemoveAt(expenseIndexToRemove);
            }

            private void _UnloadAllExpensesCommand(object parameter)
            {
                _viewModel.LoadedCategories.Clear();
                _viewModel.LoadedExpenses.Clear();
            }

            private readonly ExpensesWorkspaceViewModel _viewModel;
        }

        private IComparer<ExpenseViewModel> _expenseComparer = new LatestExpenseComparer();

        private class LatestExpenseComparer
            : IComparer<ExpenseViewModel>
        {
            public int Compare(ExpenseViewModel x, ExpenseViewModel y)
            {
                if (x == null)
                    if (y == null)
                        return 0;
                    else
                        return -1;
                else
                    if (y == null)
                        return 1;
                    else
                        return y.DateTimeTaken.CompareTo(x.DateTimeTaken);
            }
        }
    }
}