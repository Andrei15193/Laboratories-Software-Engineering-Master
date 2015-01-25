using BillPath.Models;
using BillPath.ViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BillPath.ViewModels
{
    internal class CategoryViewModel
        : ValidatableViewModel
    {
        internal CategoryViewModel(Category category, ExpensesWorkspaceViewModel expensesWorkspaceViewModel = null)
            : base(category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            Model = category;
            ExpensesWorkspaceViewModel = expensesWorkspaceViewModel;

            Expenses = new ObservableCollection<ExpenseViewModel>(Model.Expenses.Select(expense => new ExpenseViewModel(expense, expensesWorkspaceViewModel)));
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string ColorName
        {
            get
            {
                return Model.ColorName;
            }
            set
            {
                Model.ColorName = value;
                OnPropertyChanged("ColorName");
            }
        }

        public ObservableCollection<ExpenseViewModel> Expenses
        {
            get;
            private set;
        }

        internal Category Model
        {
            get;
            private set;
        }

        public ExpensesWorkspaceViewModel ExpensesWorkspaceViewModel
        {
            get
            {
                return _expensesWorkspaceViewModel;
            }
            set
            {
                _expensesWorkspaceViewModel = value;
                OnPropertyChanged("ExpensesWorkspaceViewModel");
            }
        }

        private ExpensesWorkspaceViewModel _expensesWorkspaceViewModel;
    }
}