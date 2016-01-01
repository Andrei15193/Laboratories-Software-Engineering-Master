using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public abstract class ExpenseXmlRepository
        : IExpenseRepository
    {
        private const string _rootElementName = "expenses";
        private readonly ExpenseXmlTranslator _xmlTranslator;

        public sealed class Reader
            : IExpenseReader
        {
            private readonly Stream _stream;
            private readonly XmlTranslator<Expense> _xmlTranslator;
            private readonly Predicate<Expense> _predicate;
            private XmlReader _currentXmlReader;
            private Expense _currentExpense;

            public Reader(Predicate<Expense> predicate, Stream stream, XmlTranslator<Expense> xmlTranslator)
            {
                _predicate = predicate;
                _stream = stream;
                _xmlTranslator = xmlTranslator;
                _currentXmlReader = null;
                _currentExpense = null;
            }

            public Expense Current
            {
                get
                {
                    if (_currentExpense == null)
                        throw new InvalidOperationException();

                    return _currentExpense;
                }
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);
            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
            {
                while (await _ReadNextAsync(cancellationToken) && (_predicate != null && !_predicate(_currentExpense)))
                    ;

                return _currentExpense != null;
            }
            private async Task<bool> _ReadNextAsync(CancellationToken cancellationToken)
            {
                if (_currentXmlReader == null)
                {
                    _currentXmlReader = XmlReader.Create(
                        _stream,
                        new XmlReaderSettings
                        {
                            Async = true,
                            ConformanceLevel = ConformanceLevel.Auto,
                            CloseInput = true
                        });
                    _currentExpense = await _xmlTranslator.ReadFromAsync(_currentXmlReader, cancellationToken);
                }
                else if (_currentExpense != null)
                    _currentExpense = await _xmlTranslator.ReadFromAsync(_currentXmlReader, cancellationToken);

                return _currentExpense != null;
            }

            public Task SkipAsync(int count)
                => SkipAsync(count, CancellationToken.None);
            public async Task SkipAsync(int count, CancellationToken cancellationToken)
            {
                if (count < 0)
                    throw new ArgumentException("Must be greater than or equal to zero.", nameof(count));

                while (count > 0 && await ReadAsync(cancellationToken))
                    count--;
            }

            public void Dispose()
            {
                _currentXmlReader?.Dispose();
                _stream.Dispose();
                _currentExpense = null;
            }
        }

        public ExpenseXmlRepository(IExpenseCategoryRepository expenseCategoryRepository)
        {
            _xmlTranslator = new ExpenseXmlTranslator(expenseCategoryRepository);
        }

        protected Task<Stream> GetReadStreamAsync()
            => GetReadStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken);

        protected Task<Stream> GetWriteStreamAsync()
            => GetWriteStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken);

        protected virtual int StreamBufferSize
            => 2048;

        public Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate)
            => GetReaderAsync(predicate, CancellationToken.None);
        public async Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate, CancellationToken cancellationToken)
            => new Reader(predicate, await GetReadStreamAsync(cancellationToken), _xmlTranslator);

        public Task<int> GetCountAsync(Predicate<Expense> predicate)
            => GetCountAsync(predicate, CancellationToken.None);
        public async Task<int> GetCountAsync(Predicate<Expense> predicate, CancellationToken cancellationToken)
        {
            using (var xmlReader = XmlReader.Create(
                await GetReadStreamAsync(cancellationToken),
                new XmlReaderSettings
                {
                    Async = true,
                    ConformanceLevel = ConformanceLevel.Auto,
                    CloseInput = true
                }))
                if (await xmlReader.ReadUntilAsync(_rootElementName, cancellationToken))
                    try
                    {
                        return XmlConvert.ToInt32(xmlReader.GetAttribute("count"));
                    }
                    catch (ArgumentNullException)
                    {
                        return await _GetCountAsync(null, cancellationToken);
                    }
                    catch (FormatException)
                    {
                        return await _GetCountAsync(null, cancellationToken);
                    }
                else
                    return 0;
        }
        private async Task<int> _GetCountAsync(Predicate<Expense> predicate, CancellationToken cancellationToken)
        {
            var count = 0;

            using (var reader = await GetReaderAsync(predicate, cancellationToken))
                while (await reader.ReadAsync(cancellationToken) && (predicate?.Invoke(reader.Current) ?? true))
                    count++;

            return count;
        }

        public Task SaveAsync(Expense expense)
            => SaveAsync(expense, CancellationToken.None);
        public async Task SaveAsync(Expense expense, CancellationToken cancellationToken)
        {
            using (var temporaryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(
                    temporaryStream,
                    new XmlWriterSettings
                    {
                        Async = true,
                        CloseOutput = false
                    }))
                {
                    await xmlWriter.WriteStartDocumentAsync(true);
                    await xmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    await xmlWriter.WriteAttributeStringAsync(null, "count", null, XmlConvert.ToString(await _GetCountAsync(null, cancellationToken) + 1));
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var expenseReader = await GetReaderAsync(null, cancellationToken))
                    {
                        var hasCurrent = false;
                        while (!hasCurrent && await expenseReader.ReadAsync(cancellationToken))
                            if (expenseReader.Current.DateRealized > expense.DateRealized)
                                await _xmlTranslator.WriteToAsync(xmlWriter, expenseReader.Current, cancellationToken);
                            else
                                hasCurrent = true;

                        await _xmlTranslator.WriteToAsync(xmlWriter, expense, cancellationToken);

                        if (hasCurrent)
                            do
                                await _xmlTranslator.WriteToAsync(xmlWriter, expenseReader.Current, cancellationToken);
                            while (await expenseReader.ReadAsync(cancellationToken));
                    }

                    await xmlWriter.WriteEndElementAsync();
                    await xmlWriter.WriteEndDocumentAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                temporaryStream.Seek(0, SeekOrigin.Begin);
                using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                    await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
            }
        }

        public Task RemoveAsync(Expense expense)
            => RemoveAsync(expense, CancellationToken.None);
        public async Task RemoveAsync(Expense expense, CancellationToken cancellationToken)
        {
            using (var temporaryStream = new MemoryStream())
            {
                var found = false;

                using (var xmlWriter = XmlWriter.Create(
                    temporaryStream,
                    new XmlWriterSettings
                    {
                        Async = true,
                        CloseOutput = false
                    }))
                {
                    await xmlWriter.WriteStartDocumentAsync(true);
                    await xmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    await xmlWriter.WriteAttributeStringAsync(null, "count", null, XmlConvert.ToString(await _GetCountAsync(null, cancellationToken) - 1));
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var expenseReader = await GetReaderAsync(null, cancellationToken))
                    {
                        while (!found && await expenseReader.ReadAsync(cancellationToken))
                            if (ExpenseEqualityComparer.Instance.Equals(expenseReader.Current, expense))
                                found = true;
                            else
                                await _xmlTranslator.WriteToAsync(xmlWriter, expenseReader.Current, cancellationToken);

                        while (await expenseReader.ReadAsync(cancellationToken))
                            await _xmlTranslator.WriteToAsync(xmlWriter, expenseReader.Current, cancellationToken);
                    }

                    await xmlWriter.WriteEndElementAsync();
                    await xmlWriter.WriteEndDocumentAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (found)
                {
                    temporaryStream.Seek(0, SeekOrigin.Begin);
                    using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                        await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
                }
            }
        }

        public Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory)
            => UpdateCategory(predicate, expenseCategory, CancellationToken.None);
        public async Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            using (var temporaryStream = new MemoryStream())
            {
                var found = false;

                using (var xmlWriter = XmlWriter.Create(
                    temporaryStream,
                    new XmlWriterSettings
                    {
                        Async = true,
                        CloseOutput = false
                    }))
                {
                    await xmlWriter.WriteStartDocumentAsync(true);
                    await xmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    await xmlWriter.WriteAttributeStringAsync(null, "count", null, XmlConvert.ToString(await _GetCountAsync(null, cancellationToken) - 1));
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var expenseReader = await GetReaderAsync(null, cancellationToken))
                        while (await expenseReader.ReadAsync(cancellationToken))
                        {
                            if (predicate(expenseReader.Current))
                            {
                                found = true;
                                expenseReader.Current.Category = expenseCategory;
                            }
                            await _xmlTranslator.WriteToAsync(xmlWriter, expenseReader.Current, cancellationToken);
                        }

                    await xmlWriter.WriteEndElementAsync();
                    await xmlWriter.WriteEndDocumentAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (found)
                {
                    temporaryStream.Seek(0, SeekOrigin.Begin);
                    using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                        await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
                }
            }
        }
    }
}