using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseCategoryViewModels
        : UserInterface.ViewModels.ExpenseCategoryViewModels
    {
        public ExpenseCategoryViewModels()
            : base(
                  Application.Current.GetResource<ExpenseCategoryObservableRepository>("ExpenseCategoryRepository"),
                  Application.Current.GetResource<ExpenseObservableRepository>("ExpenseRepository"))
        {
        }
    }
}