using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Income
        : ValidatableModel
    {
        public TransactionType TransactionType
        {
            get
            {
                return Models.TransactionType.Expense;
            }
        }

        [DataMember]
        public decimal Sum
        {
            get
            {
                return _sum;
            }
            set
            {
                _sum = value;
                AssertValidation(_sum > 0, "Sum", "NegativeOrZeroSum");
            }
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        public Category Category
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public DateTime DateTaken
        {
            get;
            set;
        }

        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
                AssertValidation(_account != null, "Account", "MissingAccount");
            }
        }

        private decimal _sum;
        private Account _account;
    }
}