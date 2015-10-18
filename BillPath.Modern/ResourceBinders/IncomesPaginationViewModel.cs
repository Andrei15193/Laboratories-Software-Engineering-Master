using BillPath.DataAccess;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal class IncomesPaginationViewModel
        : PaginationViewModel<Income>
    {
        public IncomesPaginationViewModel()
            : base(Application.Current.GetResource<IItemReaderProvider<Income>>(nameof(IncomeRepository)))
        {
        }
    }
}