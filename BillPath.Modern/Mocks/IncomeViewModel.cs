using BillPath.DataAccess.Xml;
using Windows.UI.Xaml;

namespace BillPath.Modern.Mocks
{
    public class IncomeViewModel
        : UserInterface.ViewModels.IncomeViewModel
    {
        public IncomeViewModel()
            : base(Application.Current.GetResource<IncomeXmlObservableRepository>())
        {
        }
    }
}