using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml.Mock
{
    public sealed class IncomeXmlRepositoryMock
        : IncomeXmlRepository, IDisposable
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();

        public void Dispose()
            => _memoryStream.Dispose();

        public override Reader GetReader()
        {
            var memoryStreamCopy = new MemoryStream();

            _memoryStream.Seek(0, SeekOrigin.Begin);
            _memoryStream.CopyTo(memoryStreamCopy);

            memoryStreamCopy.Seek(0, SeekOrigin.Begin);
            return new Reader(Enumerable.Repeat(memoryStreamCopy, 1).GetEnumerator());
        }

        public override async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            using (var resultStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(
                    resultStream,
                    new XmlWriterSettings
                    {
                        Async = true,
                        CloseOutput = false
                    }))
                {
                    await xmlWriter.WriteStartDocumentAsync(true);
                    await xmlWriter.WriteStartElementAsync(null, "incomes", null);
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var incomeReader = GetReader())
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

                resultStream.Seek(0, SeekOrigin.Begin);
                _memoryStream.Seek(0, SeekOrigin.Begin);

                await resultStream.CopyToAsync(_memoryStream, 2048, cancellationToken);
            }
        }
    }
}