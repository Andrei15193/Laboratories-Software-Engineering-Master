using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public class AppData
    {
        [DataMember]
        public AccountList Accounts
        {
            get
            {
                if (_accounts == null)
                    _accounts = new AccountList();

                return _accounts;
            }
        }

        [DataMember]
        public List<Category> Categories
        {
            get
            {
                if (_categories == null)
                    _categories = new List<Category>();

                return _categories;
            }
        }

        public bool IsValid()
        {
            return (Accounts.All(account => !account.HasErrors && account.Incomes.All(income => !income.HasErrors))
                    && Categories.All(category => !category.HasErrors && category.Expenses.All(expense => !expense.HasErrors)));
        }

        private AccountList _accounts = null;
        private List<Category> _categories = null;
    }
}