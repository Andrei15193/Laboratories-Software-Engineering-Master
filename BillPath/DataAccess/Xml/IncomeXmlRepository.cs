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
        private static readonly XmlTranslator<Income> _translator = new IncomeXmlTranslator();

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
                    _currentIncome = await _translator.ReadFromAsync(_currentXmlReader, cancellationToken);
                }
                else if (_currentIncome != null)
                    _currentIncome = await _translator.ReadFromAsync(_currentXmlReader, cancellationToken);

                return _currentIncome != null;
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

        public Task<int> GetCountAsync()
            => GetCountAsync(CancellationToken.None);
        public async Task<int> GetCountAsync(CancellationToken cancellationToken)
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
                        return await _GetCountAsync(cancellationToken);
                    }
                    catch (FormatException)
                    {
                        return await _GetCountAsync(cancellationToken);
                    }
                else
                    return 0;
        }
        private async Task<int> _GetCountAsync(CancellationToken cancellationToken)
        {
            var count = 0;

            using (var reader = await GetReaderAsync(cancellationToken))
                while (await reader.ReadAsync(cancellationToken))
                    count++;

            return count;
        }

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
                    await xmlWriter.WriteAttributeStringAsync(null, "count", null, XmlConvert.ToString(await GetCountAsync(cancellationToken) + 1));
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var incomeReader = await GetReaderAsync(cancellationToken))
                    {
                        var hasCurrent = false;
                        while (!hasCurrent && await incomeReader.ReadAsync(cancellationToken))
                            if (incomeReader.Current.DateRealized > income.DateRealized)
                                await _translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
                            else
                                hasCurrent = true;

                        await _translator.WriteToAsync(xmlWriter, income, cancellationToken);

                        if (hasCurrent)
                            do
                                await _translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
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
                    await xmlWriter.WriteAttributeStringAsync(null, "count", null, XmlConvert.ToString(await GetCountAsync(cancellationToken) - 1));
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var incomeReader = await GetReaderAsync(cancellationToken))
                    {
                        while (!found && await incomeReader.ReadAsync(cancellationToken))
                            if (IncomeEqualityComparer.Instance.Equals(incomeReader.Current, income))
                                found = true;
                            else
                                await _translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);

                        while (await incomeReader.ReadAsync(cancellationToken))
                            await _translator.WriteToAsync(xmlWriter, incomeReader.Current, cancellationToken);
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