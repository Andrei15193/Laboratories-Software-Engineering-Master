using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class ExpenseXmlTranslator
        : XmlTranslator<Expense>
    {
        private const string _dateRealizedFormatString = "yyyy/M/d HH:mm:ss:fffffff zzz";
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        public ExpenseXmlTranslator(IExpenseCategoryRepository expenseCategoryRepository)
        {
            if (expenseCategoryRepository == null)
                throw new ArgumentNullException(nameof(expenseCategoryRepository));

            _expenseCategoryRepository = expenseCategoryRepository;
        }

        public override async Task<Expense> ReadFromAsync(XmlReader xmlReader, CancellationToken cancellationToken)
        {
            if (!await xmlReader.ReadUntilAsync(nameof(Expense).ToXmlName(), cancellationToken))
                return null;

            var expense = new Expense();

            expense.DateRealized = XmlConvert.ToDateTimeOffset(
                xmlReader.GetAttribute(nameof(Expense.DateRealized).ToXmlName()),
                _dateRealizedFormatString).ToLocalTime();
            expense.Description = xmlReader.GetAttribute(nameof(Expense.Description).ToXmlName());

            var categoryName = xmlReader.GetAttribute(nameof(Expense.Category).ToXmlName());
            var categories = await _expenseCategoryRepository.GetAllAsync(cancellationToken);
            expense.Category = categories.SingleOrDefault(category => string.Equals(
                categoryName,
                category.Name,
                StringComparison.OrdinalIgnoreCase));

            xmlReader.ReadToDescendant(nameof(Expense.Amount).ToXmlName());
            expense.Amount = _ReadAmountFrom(xmlReader);

            return expense;
        }
        private Amount _ReadAmountFrom(XmlReader xmlReader)
            => new Amount(
                Convert.ToDecimal(xmlReader.GetAttribute(nameof(Amount.Value).ToXmlName())),
                new Currency(
                    xmlReader.GetAttribute(nameof(Currency.IsoCode).ToXmlName()),
                    xmlReader.GetAttribute(nameof(Currency.Symbol).ToXmlName())));

        public override async Task WriteToAsync(XmlWriter xmlWriter, Expense expense, CancellationToken cancellationToken)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (expense == null)
                throw new ArgumentNullException(nameof(expense));

            await xmlWriter.WriteStartElementAsync(
                null,
                nameof(Expense).ToXmlName(),
                null);
            xmlWriter.WriteAttributeString(
                nameof(Expense.DateRealized).ToXmlName(),
                XmlConvert.ToString(
                    expense.DateRealized.ToUniversalTime(),
                    _dateRealizedFormatString));
            if (!string.IsNullOrWhiteSpace(expense.Description))
                xmlWriter.WriteAttributeString(
                    nameof(Expense.Description).ToXmlName(),
                    expense.Description);
            xmlWriter.WriteAttributeString(
                nameof(Expense.Category).ToXmlName(),
                expense.Category.Name);
            cancellationToken.ThrowIfCancellationRequested();

            await _WriteAmountAsync(xmlWriter, expense.Amount);
            cancellationToken.ThrowIfCancellationRequested();

            await xmlWriter.WriteEndElementAsync();
            cancellationToken.ThrowIfCancellationRequested();
        }
        private static async Task _WriteAmountAsync(XmlWriter xmlWriter, Amount amount)
        {
            await xmlWriter.WriteStartElementAsync(null, nameof(Expense.Amount).ToXmlName(), null);

            xmlWriter.WriteAttributeString(nameof(Amount.Value).ToXmlName(), Convert.ToString(amount.Value));
            xmlWriter.WriteAttributeString(nameof(Currency.IsoCode).ToXmlName(), amount.Currency.IsoCode);
            xmlWriter.WriteAttributeString(nameof(Currency.Symbol).ToXmlName(), amount.Currency.Symbol);

            await xmlWriter.WriteEndElementAsync();
        }
    }
}