using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillPath.Models
{
    public abstract class Transaction<TTransaction>
        : IValidatableObject, ICloneable<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        public string Description
        {
            get;
            set;
        }
        public Amount Amount
        {
            get;
            set;
        }
        public DateTimeOffset DateRealized
        {
            get;
            set;
        }

        public virtual TTransaction Clone()
        {
            return (TTransaction)MemberwiseClone();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.Currency == default(Currency))
                yield return new ValidationResult(Strings.Transaction.Amount_MustHaveCurrency, new[] { nameof(Amount) });
            if (Amount.Value <= 0)
                yield return new ValidationResult(Strings.Transaction.Amount_ValueMustBeStrictlyPositive, new[] { nameof(Amount) });

            foreach (var validationResult in OnValidated(validationContext))
                yield return validationResult;
        }

        protected virtual IEnumerable<ValidationResult> OnValidated(ValidationContext validationContext)
        {
            yield break;
        }
    }
}