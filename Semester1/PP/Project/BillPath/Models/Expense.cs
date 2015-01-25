using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Expense
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
                AssertValidation(_sum >= 0, "Sum", "NegativeSum");
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
                return _category;
            }
            set
            {
                _category = value;
                AssertValidation(_category != null, "Category", "MissingCategory");
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
                _accountName = (_account == null ? null : _account.CurrencyName);
                AssertValidation(_account != null, "Account", "MissingAccount");
            }
        }

        [DataMember]
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }
        
        private Category _category;
        private Account _account;
        private decimal _sum;
        private string _accountName;
    }
}