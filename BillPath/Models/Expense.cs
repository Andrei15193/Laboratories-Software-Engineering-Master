using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Expense
        : Transaction<Expense>, IValidatableObject
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

        public override Expense Clone()
        {
            return (Expense)MemberwiseClone();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.Value < 0)
                yield return new ValidationResult(Strings.Expense.Amount_MustBePositive, new[] { nameof(Amount) });
        }
    }
}