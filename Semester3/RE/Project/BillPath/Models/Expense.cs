using System.ComponentModel.DataAnnotations;

namespace BillPath.Models
{
    public class Expense
        : Transaction<Expense>
    {
        [Required(
            ErrorMessageResourceName = nameof(Strings.Expense.Category_Required),
            ErrorMessageResourceType = typeof(Strings.Expense))]
        public ExpenseCategory Category
        {
            get;
            set;
        }
    }
}