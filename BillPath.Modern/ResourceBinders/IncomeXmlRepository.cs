using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class IncomeXmlRepository
        : DataAccess.Xml.IncomeXmlFileRepository
    {
        public IncomeXmlRepository()
            : base(Application.Current.GetResource<string>("IncomesFileName"))
        {
        }
    }
}