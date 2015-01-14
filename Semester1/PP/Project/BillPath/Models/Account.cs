using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Account
        : ValidatableModel
    {
        [DataMember]
        public List<Income> Incomes
        {
            get
            {
                if (_incomes == null)
                    _incomes = new List<Income>();

                return _incomes;
            }
        }

        [DataMember]
        public string CurrencyName
        {
            get;
            set;
        }

        internal bool HasDuplicateCurrency
        {
            get
            {
                return HasValidationError("Currency", "DuplicateCurrency");
            }
            set
            {
                AssertValidation(!value, "Currency", "DuplicateCurrency");
            }
        }

        private List<Income> _incomes = null;
    }
}