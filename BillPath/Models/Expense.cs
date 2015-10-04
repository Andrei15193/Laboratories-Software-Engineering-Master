using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Expense
        : Transaction<Expense>
    {
        [DataMember]
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