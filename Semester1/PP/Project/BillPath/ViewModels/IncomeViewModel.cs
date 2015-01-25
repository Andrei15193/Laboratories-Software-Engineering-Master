using BillPath.Models;
using BillPath.ViewModels.Core;
using System;

namespace BillPath.ViewModels
{
    internal class IncomeViewModel
        : ValidatableViewModel
    {
        internal IncomeViewModel(Income income, ExpensesWorkspaceViewModel expensesWorkspaceViewModel)
            : base(income)
        {
            if (income == null)
                throw new ArgumentNullException("income");
            if (expensesWorkspaceViewModel == null)
                throw new ArgumentNullException("expensesWorkspaceViewModel");

            Model = income;
            _expensesWorkspaceViewModel = expensesWorkspaceViewModel;
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
        public AccountViewModel Account
        {
            get
            {
                return _expensesWorkspaceViewModel.GetViewModelForAccount(Model.Account);
            }
            set
            {
                if (value != null)
                    Model.Account = value.Model;
                else
                    Model.Account = null;

                OnPropertyChanged("Account");
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

        public DateTime DateTimeTaken
        {
            get
            {
                return Model.DateTaken;
            }
        }

        internal Income Model
        {
            get;
            private set;
        }

        private readonly ExpensesWorkspaceViewModel _expensesWorkspaceViewModel;
    }
}