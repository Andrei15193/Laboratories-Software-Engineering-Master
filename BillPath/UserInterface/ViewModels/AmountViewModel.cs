using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class AmountViewModel
        : ViewModel<Amount>
    {
        public AmountViewModel(Amount model)
            : base(model)
        {
        }

        public Currency Currency
        {
            get
            {
                return Model.Currency;
            }
            set
            {
                Model = new Amount(Model.Value, value);
                OnPropertyChanged();
            }
        }

        public decimal Value
        {
            get
            {
                return Model.Value;
            }
            set
            {
                Model = new Amount(value, Model.Currency);
                OnPropertyChanged();
            }
        }
    }
}