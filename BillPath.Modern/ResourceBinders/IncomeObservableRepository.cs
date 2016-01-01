using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class IncomeObservableRepository
        : DataAccess.IncomeObservableRepository
    {
        public IncomeObservableRepository()
            : base(Application.Current.GetResource<DataAccess.IIncomeRepository>())
        {
        }
    }
}