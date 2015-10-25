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
            Amount = new AmountViewModel(model.Amount);
            Amount.PropertyChanged +=
                delegate
                {
                    Model.Amount = Amount.Model;
                    ValidateModel();
                };
        }

        public AmountViewModel Amount
        {
            get;
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