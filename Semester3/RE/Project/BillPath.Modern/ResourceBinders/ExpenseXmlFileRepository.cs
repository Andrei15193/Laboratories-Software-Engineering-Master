using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal sealed class ExpenseXmlFileRepository
        : DataAccess.Xml.ExpenseXmlFileRepository
    {
        public ExpenseXmlFileRepository()
            : base(
                  Application.Current.GetResource<string>("ExpensesFileName"),
                  Application.Current.GetResource<IExpenseCategoryRepository>("ExpenseCategoryRepository"))
        {
        }
    }
}