using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseCategoryViewModel
        : UserInterface.ViewModels.ExpenseCategoryViewModel
    {
        public ExpenseCategoryViewModel()
            : base(Application.Current.GetResource<IExpenseCategoryRepository>("ExpenseCategoryRepository"))
        {
        }
    }
}