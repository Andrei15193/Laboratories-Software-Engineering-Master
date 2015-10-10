using System;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomeViewModel
        : ViewModel<Income>
    {
        public IncomeViewModel(Income model)
            : base(model)
        {
        }

        public Amount Amount
        {
            get
            {
                return Model.Amount;
            }
            set
            {
                Model.Amount = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public DateTimeOffset DateRealized
        {
            get
            {
                return Model.DateRealized;
            }
            set
            {
                Model.DateRealized = value;
                OnPropertyChanged();
            }
        }
    }
}