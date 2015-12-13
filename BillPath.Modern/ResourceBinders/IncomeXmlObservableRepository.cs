using BillPath.DataAccess.Xml;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class IncomeXmlObservableRepository
        : DataAccess.Xml.IncomeXmlObservableRepository
    {
        public IncomeXmlObservableRepository()
            : base(Application.Current.GetResource<IIncomeXmlRepository>(nameof(IncomeXmlRepository)))
        {
        }
    }
}