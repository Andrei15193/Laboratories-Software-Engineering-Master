using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal class IncomesViewModel
        : UserInterface.ViewModels.IncomesViewModel
    {
        public IncomesViewModel()
            : base(Application.Current.GetResource<IIncomeRepository>(nameof(IncomeRepository)))
        {
        }
    }
}