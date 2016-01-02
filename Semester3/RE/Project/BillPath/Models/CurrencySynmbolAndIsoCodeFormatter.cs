namespace BillPath.Models
{
    public class CurrencySynmbolAndIsoCodeFormatter
        : ICurrencyFormatter
    {
        public string Format(Currency currency)
        {
            if (currency == default(Currency))
                return string.Empty;
            else
                return $"{currency.Symbol}({currency.IsoCode})";
        }
    }
}