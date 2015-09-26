using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class ExpenseCategory
        : ICloneable<ExpenseCategory>
    {
        [DataMember]
        [Required(
            ErrorMessageResourceName = nameof(Name) + "_Required",
            ErrorMessageResourceType = typeof(Strings.ExpenseCategory)
            )]
        public string Name
        {
            get;
            set;
        }
        [DataMember]
        public ArgbColor Color
        {
            get;
            set;
        }

        public ExpenseCategory Clone()
        {
            return (ExpenseCategory)MemberwiseClone();
        }
    }
}