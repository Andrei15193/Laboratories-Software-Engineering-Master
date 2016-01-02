using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseViewModel
        : UserInterface.ViewModels.ExpenseViewModel
    {
        public ExpenseViewModel()
            : base(Application.Current.GetResource<IExpenseRepository>("ExpenseRepository"))
        {
        }
    }
}