namespace BillPath.Models
{
    public class CurrencyIsoCodeOnlyFormatter
        : ICurrencyFormatter
    {
        public string Format(Currency currency)
        {
            return currency.IsoCode;
        }
    }
}