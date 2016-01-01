using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseCategoryXmlFileRepository
        : DataAccess.Xml.ExpenseCategoryXmlFileRepository
    {
        public ExpenseCategoryXmlFileRepository()
            : base(Application.Current.GetResource<string>("ExpenseCategoriesFileName"))
        {
        }
    }
}