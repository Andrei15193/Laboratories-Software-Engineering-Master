using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseObservableRepository
        : DataAccess.ExpenseObservableRepository
    {
        public ExpenseObservableRepository()
            : base(Application.Current.GetResource<DataAccess.IExpenseRepository>("ExpenseRepositoryImplementation"))
        {
        }
    }
}