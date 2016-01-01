using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class IncomeXmlTranslator
        : XmlTranslator<Income>
    {
        private const string _dateRealizedFormatString = "yyyy/M/d HH:mm:ss:fffffff zzz";

        public override async Task<Income> ReadFromAsync(XmlReader xmlReader, CancellationToken cancellationToken)
        {
            if (!await xmlReader.ReadUntilAsync(nameof(Income).ToXmlName(), cancellationToken))
                return null;

            var income = new Income();

            income.DateRealized = XmlConvert.ToDateTimeOffset(
                xmlReader.GetAttribute(nameof(Income.DateRealized).ToXmlName()),
                _dateRealizedFormatString).ToLocalTime();
            income.Description = xmlReader.GetAttribute(nameof(Income.Description).ToXmlName());

            xmlReader.ReadToDescendant(nameof(Income.Amount).ToXmlName());
            income.Amount = _ReadAmountFrom(xmlReader);

            return income;
        }
        private Amount _ReadAmountFrom(XmlReader xmlReader)
            => new Amount(
                Convert.ToDecimal(xmlReader.GetAttribute(nameof(Amount.Value).ToXmlName())),
                new Currency(
                    xmlReader.GetAttribute(nameof(Currency.IsoCode).ToXmlName()),
                    xmlReader.GetAttribute(nameof(Currency.Symbol).ToXmlName())));

        public override async Task WriteToAsync(XmlWriter xmlWriter, Income income, CancellationToken cancellationToken)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            await xmlWriter.WriteStartElementAsync(
                null,
                nameof(Income).ToXmlName(),
                null);
            xmlWriter.WriteAttributeString(
                nameof(Income.DateRealized).ToXmlName(),
                XmlConvert.ToString(
                    income.DateRealized.ToUniversalTime(),
                    _dateRealizedFormatString));
            if (!string.IsNullOrWhiteSpace(income.Description))
                xmlWriter.WriteAttributeString(
                    nameof(Income.Description).ToXmlName(),
                    income.Description);
            cancellationToken.ThrowIfCancellationRequested();

            await _WriteAmountAsync(xmlWriter, income.Amount);
            cancellationToken.ThrowIfCancellationRequested();

            await xmlWriter.WriteEndElementAsync();
            cancellationToken.ThrowIfCancellationRequested();
        }
        private static async Task _WriteAmountAsync(XmlWriter xmlWriter, Amount amount)
        {
            await xmlWriter.WriteStartElementAsync(null, nameof(Income.Amount).ToXmlName(), null);

            xmlWriter.WriteAttributeString(nameof(Amount.Value).ToXmlName(), Convert.ToString(amount.Value));
            xmlWriter.WriteAttributeString(nameof(Currency.IsoCode).ToXmlName(), amount.Currency.IsoCode);
            xmlWriter.WriteAttributeString(nameof(Currency.Symbol).ToXmlName(), amount.Currency.Symbol);

            await xmlWriter.WriteEndElementAsync();
        }
    }
}