using BillPath.Models;
using BillPath.ViewModels.Core;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace BillPath.ViewModels
{
    internal class AccountViewModel
        : ValidatableViewModel
    {
        internal AccountViewModel(Account account, ExpensesWorkspaceViewModel expensesWorkspaceViewModel)
            : base(account)
        {
            if (account == null)
                throw new ArgumentNullException("account");
            if (expensesWorkspaceViewModel == null)
                throw new ArgumentNullException("expensesWorkspaceViewModel");

            _expensesWorkspaceViewModel = expensesWorkspaceViewModel;

            Model = account;
            Incomes = new ObservableCollection<IncomeViewModel>(account.Incomes.Select(income => new IncomeViewModel(income, expensesWorkspaceViewModel)));
            Incomes.CollectionChanged += _TransactionCollectionChanged;

            expensesWorkspaceViewModel.Categories.CollectionChanged += (sender, e) =>
                                                                       {
                                                                           if (e.NewItems != null)
                                                                               foreach (CategoryViewModel newCategoryViewModel in e.NewItems.OfType<CategoryViewModel>())
                                                                                   newCategoryViewModel.Expenses.CollectionChanged += _TransactionCollectionChanged;

                                                                           if (e.OldItems != null)
                                                                               foreach (CategoryViewModel oldCategoryViewModel in e.OldItems.OfType<CategoryViewModel>())
                                                                                   oldCategoryViewModel.Expenses.CollectionChanged -= _TransactionCollectionChanged;

                                                                           OnPropertyChanged("TotalAvailable");
                                                                       };
            foreach (CategoryViewModel categoryViewModel in expensesWorkspaceViewModel.Categories)
                categoryViewModel.Expenses.CollectionChanged += _TransactionCollectionChanged;

            RemoveIncomesCommand = new RemoveIncomes(expensesWorkspaceViewModel);
        }

        public RemoveIncomes RemoveIncomesCommand
        {
            get;
            private set;
        }

        public Currency Currency
        {
            get
            {
                return Currencies.GetCurrencyByName(Model.CurrencyName);
            }
        }

        public decimal TotalAvailable
        {
            get
            {
                return (Incomes.Sum(income => income.Sum) - _expensesWorkspaceViewModel.Categories
                                                                                       .SelectMany(category => category.Expenses)
                                                                                       .Where(expense => expense.Account == this)
                                                                                       .Sum(expense => expense.Sum));
            }
        }

        public ObservableCollection<IncomeViewModel> Incomes
        {
            get;
            private set;
        }

        internal Account Model
        {
            get;
            private set;
        }

        private void _TransactionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("TotalAvailable");
        }

        private readonly ExpensesWorkspaceViewModel _expensesWorkspaceViewModel;

        internal sealed class RemoveIncomes
            : Command<ExpensesWorkspaceViewModel, IEnumerable>
        {
            internal RemoveIncomes(ExpensesWorkspaceViewModel viewModel)
                : base(viewModel)
            {
            }

            public IEnumerable Incomes
            {
                get
                {
                    return _incomes;
                }
                set
                {
                    _incomes = value;
                    OnCanExecuteChanged();
                }
            }

            public override bool CanExecute(IEnumerable parameter)
            {
                return (_incomes != null && _incomes.OfType<IncomeViewModel>().Any());
            }

            public override void Execute(IEnumerable parameter)
            {
                foreach (IncomeViewModel incomeViewModel in  _incomes.OfType<IncomeViewModel>().ToList())
                {
                    incomeViewModel.Account.Incomes.Remove(incomeViewModel);
                    incomeViewModel.Account.Model.Incomes.Remove(incomeViewModel.Model);
                }
            }

            private IEnumerable _incomes;
        }
    }
}