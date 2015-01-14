using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BillPath.Models
{
    public class Currencies
    {
        public static readonly Currency RON = new Currency { Name = "RON", Symbol = "RON" };
        public static readonly Currency EUR = new Currency { Name = "EUR", Symbol = "€" };
        public static readonly Currency USD = new Currency { Name = "USD", Symbol = "$" };
        
        public static IEnumerable<Currency> AllCurrencies
        {
            get
            {
                return _currenciesByName.Values;
            }
        }

        public static Currency GetCurrencyByName(string name)
        {
            return _currenciesByName[name];
        }

        private static IReadOnlyDictionary<string, Currency> _currenciesByName = _GetCurrenciesByName();

        private static IReadOnlyDictionary<string, Currency> _GetCurrenciesByName()
        {
            IDictionary<string, Currency> currenciesByName = new SortedDictionary<string, Currency>(StringComparer.Ordinal);

            currenciesByName.Add(RON.Name, RON);
            currenciesByName.Add(EUR.Name, EUR);
            currenciesByName.Add(USD.Name, USD);

            return new ReadOnlyDictionary<string, Currency>(currenciesByName);
        }
    }
}