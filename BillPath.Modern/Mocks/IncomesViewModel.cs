using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.Mocks
{
    internal class IncomesViewModel
        : UserInterface.ViewModels.IncomesViewModel
    {
        public IncomesViewModel()
            : base((IIncomeRepository)Application.Current.Resources[nameof(IncomeRepository)])
        {
        }
    }
}