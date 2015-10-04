using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Income
        : Transaction<Income>, IValidatableObject
    {
        public override Income Clone()
        {
            return new Income
            {
                Description = Description,
                Amount = Amount,
                DateRealized = DateRealized
            };
        }

        protected override IEnumerable<ValidationResult> OnValidated(ValidationContext validationContext)
        {
            if (Amount.Value <= 0)
                yield return new ValidationResult(Strings.Income.Amount_MustBeStrictlyPositive, new[] { nameof(Amount) });
        }
    }
}