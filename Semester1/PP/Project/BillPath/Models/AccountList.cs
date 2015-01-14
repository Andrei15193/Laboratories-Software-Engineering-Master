using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BillPath.Models
{
    public class AccountList
        : ValidatableModel, IList<Account>
    {
        public int IndexOf(Account account)
        {
            if (account == null)
                return -1;
            else
                return _accounts.IndexOf(account);
        }

        public void Insert(int index, Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            _accounts.Insert(index, account);
            OnUpdateValidationList();
        }

        public void RemoveAt(int index)
        {
            _accounts.RemoveAt(index);
            OnUpdateValidationList();
        }

        public Account this[int index]
        {
            get
            {
                return _accounts[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _accounts[index] = value;
                OnUpdateValidationList();
            }
        }

        public void Add(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            _accounts.Add(account);
            OnUpdateValidationList();
        }

        public void Clear()
        {
            _accounts.Clear();
            OnUpdateValidationList();
        }

        public bool Contains(Account account)
        {
            return (account != null && _accounts.Contains(account));
        }

        public void CopyTo(Account[] array, int arrayIndex)
        {
            _accounts.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _accounts.Count;
            }
        }

        bool ICollection<Account>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(Account account)
        {
            if (account != null && _accounts.Remove(account))
            {
                OnUpdateValidationList();
                return true;
            }
            else
                return false;
        }

        public IEnumerator<Account> GetEnumerator()
        {
            return _accounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnUpdateValidationList()
        {
            bool hasDuplicateCurrencies = false;

            foreach (Account account in this.GroupBy(account => account.CurrencyName)
                                            .Where(accountsByCurrency => accountsByCurrency.Count() > 1)
                                            .SelectMany(Enumerable.AsEnumerable))
            {
                account.HasDuplicateCurrency = true;
                hasDuplicateCurrencies = true;
            }

            AssertValidation(hasDuplicateCurrencies, null, "DuplicateCurrencies", typeof(Account));
        }

        private readonly List<Account> _accounts = new List<Account>();
    }
}