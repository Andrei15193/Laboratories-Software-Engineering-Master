using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Expense
        : Transaction<Expense>
    {
        [DataMember]
        public ExpenseCategory Category
        {
            get;
            set;
        }

        public override Expense Clone()
        {
            return new Expense
            {
                Currency = Currency,
                Description = Description,
                Amount = Amount,
                DateRealized = DateRealized,
                Category = Category
            };
        }
    }
}