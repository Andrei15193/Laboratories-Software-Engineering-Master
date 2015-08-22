using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class ExpenseCategory
        : ICloneable<ExpenseCategory>
    {
        [DataMember]
        public string Name
        {
            get;
            set;
        }
        [DataMember]
        public ArgbColor RgbColor
        {
            get;
            set;
        }

        public ExpenseCategory Clone()
        {
            return new ExpenseCategory
            {
                Name = Name,
                RgbColor = RgbColor
            };
        }
    }
}