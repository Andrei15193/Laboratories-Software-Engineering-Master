namespace BillPath.Models
{
    public class CurrencySynmbolAndIsoCodeFormatter
        : ICurrencyFormatter
    {
        public string Format(Currency currency)
        {
            return $"{currency.Symbol}({currency.IsoCode})";
        }
    }
}