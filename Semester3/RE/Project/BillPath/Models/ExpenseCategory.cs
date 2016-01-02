using System.ComponentModel.DataAnnotations;

namespace BillPath.Models
{
    public class ExpenseCategory
        : ICloneable<ExpenseCategory>
    {
        [Required(
            ErrorMessageResourceName = nameof(Strings.ExpenseCategory.Name_Required),
            ErrorMessageResourceType = typeof(Strings.ExpenseCategory)
            )]
        public string Name
        {
            get;
            set;
        }
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