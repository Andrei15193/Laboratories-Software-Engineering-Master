namespace BillPath.Models
{
    public class CurrencySymbolOnlyFormatter
        : ICurrencyFormatter
    {
        public string Format(Currency currency)
        {
            return currency.Symbol;
        }
    }
}