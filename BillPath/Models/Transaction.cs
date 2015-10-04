using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public abstract class Transaction<TTransaction>
        : IValidatableObject, ICloneable<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        [DataMember]
        public string Description
        {
            get;
            set;
        }
        [DataMember]
        public Amount Amount
        {
            get;
            set;
        }
        [DataMember]
        public DateTimeOffset DateRealized
        {
            get;
            set;
        }

        public abstract TTransaction Clone();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.Currency == default(Currency))
                yield return new ValidationResult(Strings.Transaction.Amount_MustHaveCurrency, new[] { nameof(Amount) });

            foreach (var validationResult in OnValidated(validationContext))
                yield return validationResult;
        }

        protected virtual IEnumerable<ValidationResult> OnValidated(ValidationContext validationContext)
        {
            yield break;
        }
    }
}