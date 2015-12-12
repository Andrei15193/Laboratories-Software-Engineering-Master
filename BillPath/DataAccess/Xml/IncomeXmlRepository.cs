using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public abstract class IncomeXmlRepository
        : IIncomeXmlRepository
    {
        private const string _rootElementName = "incomes";
        protected static XmlTranslator<Income> Translator { get; } = new IncomeXmlTranslator();

        public sealed class Reader
            : IIncomeXmlReader
        {
            private readonly Stream _stream;
            private XmlReader _currentXmlReader;
            private Income _currentIncome;

            public Reader(Stream stream)
            {
                _stream = stream;
                _currentXmlReader = null;
                _currentIncome = null;
            }

            public Income Current
            {
                get
                {
                    if (_currentIncome == null)
                        throw new InvalidOperationException();

                    return _currentIncome;
                }
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);
            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
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
                    _currentIncome = await Translator.ReadFromAsync(_currentXmlReader, cancellationToken);
                }
                else if (_currentIncome != null)
                    _currentIncome = await Translator.ReadFromAsync(_currentXmlReader, cancellationToken);

                return _currentIncome != null;
            }

            public void Dispose()
            {
                _currentXmlReader?.Dispose();
                _stream.Dispose();
                _currentIncome = null;
            }
        }

        protected Task<Stream> GetReadStreamAsync()
            => GetReadStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken);

        protected Task<Stream> GetWriteStreamAsync()
            => GetWriteStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken);

        protected virtual int StreamBufferSize
            => 2048;

        public Task<IIncomeXmlReader> GetReaderAsync()
            => GetReaderAsync(CancellationToken.None);
        public async Task<IIncomeXmlReader> GetReaderAsync(CancellationToken cancellationToken)
            => new Reader(await GetReadStreamAsync(cancellationToken));

        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
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
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var incomeReader = await GetReaderAsync(cancellationToken))
                    {
                        var hasCurrent = false;
                        while (!hasCurrent && await incomeReader.ReadAsync(cancellationToken))
                            if (incomeReader.Current.DateRealized > income.DateRealized)
                                await Translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
                            else
                                hasCurrent = true;

                        await Translator.WriteToAsync(xmlWriter, income, cancellationToken);

                        if (hasCurrent)
                            do
                                await Translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
                            while (await incomeReader.ReadAsync(cancellationToken));
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

        public Task RemoveAsync(Income income)
            => RemoveAsync(income, CancellationToken.None);
        public async Task RemoveAsync(Income income, CancellationToken cancellationToken)
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
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var incomeReader = await GetReaderAsync(cancellationToken))
                    {
                        var found = false;

                        while (!found && await incomeReader.ReadAsync(cancellationToken))
                            if (IncomeEqualityComparer.Instance.Equals(incomeReader.Current, income))
                                found = true;
                            else
                                await Translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);

                        while (await incomeReader.ReadAsync(cancellationToken))
                            await Translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
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
    }
}