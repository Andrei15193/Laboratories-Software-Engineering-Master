using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Expense
        : Transaction
    {
        [DataMember]
        public ExpenseCategory Category
        {
            get;
            set;
        }
    }
}