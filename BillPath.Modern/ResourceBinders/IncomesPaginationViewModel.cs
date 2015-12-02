using BillPath.DataAccess;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal class IncomesPaginationViewModel
        : PaginationViewModel<IncomeViewModel>
    {
        public IncomesPaginationViewModel()
            : base(new IncomeViewModelReaderProvider(Application.Current.GetResource<IIncomesRepository>(nameof(IncomesRepository))))
        {
        }
    }
}