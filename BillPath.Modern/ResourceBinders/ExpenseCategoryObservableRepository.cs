using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseCategoryObservableRepository
        : DataAccess.ExpenseCategoryObservableRepository
    {
        public ExpenseCategoryObservableRepository()
            : base(Application.Current.GetResource<DataAccess.IExpenseCategoryRepository>("ExpenseCategoryRepositoryImplementation"))
        {
        }
    }
}