using BillPath.Models;
using BillPath.ViewModels.Core;
using System;

namespace BillPath.ViewModels
{
    internal class ExpenseViewModel
        : ValidatableViewModel
    {
        internal ExpenseViewModel(Expense expense, ExpensesWorkspaceViewModel workspaceViewModel)
            : base(expense)
        {
            if (expense == null)
                throw new ArgumentNullException("expense");
            if (workspaceViewModel == null)
                throw new ArgumentNullException("workspaceViewModel");

            Model = expense;
            _workspaceViewModel = workspaceViewModel;
        }

        public TransactionType TransactionType
        {
            get
            {
                return Models.TransactionType.Expense;
            }
        }

        public decimal Sum
        {
            get
            {
                return Model.Sum;
            }
            set
            {
                Model.Sum = value;
                OnPropertyChanged("Sum");
            }
        }

        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public CategoryViewModel Category
        {
            get
            {
                return _workspaceViewModel.GetViewModelForCategory(Model.Category);
            }
            set
            {
                if (value != null)
                    Model.Category = value.Model;
                else
                    Model.Category = null;

                OnPropertyChanged("Category");
            }
        }

        public DateTimeOffset DateTaken
        {
            get
            {
                return Model.DateTaken.Date;
            }
            set
            {
                Model.DateTaken = value.Date.AddMinutes(Model.DateTaken.TimeOfDay.TotalMinutes);
                OnPropertyChanged("DateTaken");
            }
        }

        public TimeSpan TimeTaken
        {
            get
            {
                return Model.DateTaken.TimeOfDay;
            }
            set
            {
                Model.DateTaken = Model.DateTaken.Date.AddMinutes(value.TotalMinutes);
                OnPropertyChanged("TimeTaken");
            }
        }

        public AccountViewModel Account
        {
            get
            {
                return _workspaceViewModel.GetViewModelForAccount(Model.Account);
            }
            set
            {
                Model.Account = value.Model;
                OnPropertyChanged("Account");
            }
        }

        public DateTime DateTimeTaken
        {
            get
            {
                return Model.DateTaken;
            }
        }

        internal Expense Model
        {
            get;
            private set;
        }

        private readonly ExpensesWorkspaceViewModel _workspaceViewModel;
    }
}