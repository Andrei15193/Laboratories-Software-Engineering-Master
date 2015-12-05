using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public abstract class IncomeXmlRepository
    {
        protected static XmlTranslator<Income> Translator { get; } = new IncomeXmlTranslator();

        public sealed class Reader
            : IDisposable
        {
            private XmlReader _currentXmlReader;
            private readonly IEnumerator<Stream> _streamEnumerator;
            private Income _currentIncome;

            public Income Current
            {
                get
                {
                    if (_currentIncome == null)
                        throw new InvalidOperationException();

                    return _currentIncome;
                }
            }

            public Reader(IEnumerator<Stream> streamEnumerator)
            {
                _currentIncome = null;
                _currentXmlReader = null;
                _streamEnumerator = streamEnumerator;
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);
            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
            {
                if (_currentXmlReader != null)
                    _currentIncome = await Translator.ReadFromAsync(_currentXmlReader, cancellationToken);

                while (_currentIncome == null && _streamEnumerator.MoveNext())
                {
                    _currentXmlReader?.Dispose();
                    _currentXmlReader = XmlReader.Create(
                        _streamEnumerator.Current,
                        new XmlReaderSettings
                        {
                            Async = true,
                            ConformanceLevel = ConformanceLevel.Auto,
                            CloseInput = true
                        });
                    _currentIncome = await Translator.ReadFromAsync(_currentXmlReader, cancellationToken);
                }

                return _currentIncome != null;
            }

            public void Dispose()
            {
                _currentXmlReader?.Dispose();
                _streamEnumerator.Dispose();
            }
        }

        public abstract Reader GetReader();

        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public abstract Task SaveAsync(Income income, CancellationToken cancellationToken);
    }
}