using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class Category
        : ValidatableModel
    {
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = (value ?? string.Empty);
                AssertValidation(!string.IsNullOrWhiteSpace(_name), "Name", "WhiteSpaceName");
            }
        }

        [DataMember]
        public string ColorName
        {
            get;
            set;
        }

        [DataMember]
        public List<Expense> Expenses
        {
            get
            {
                if (_expenses == null)
                    _expenses = new List<Expense>();

                return _expenses;
            }
        }

        private string _name;
        private List<Expense> _expenses = null;
    }
}