using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess.Xml
{
    public sealed class IncomeXmlMockRepository
        : IncomeXmlRepository, IDisposable
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();

        public void Dispose()
            => _memoryStream.Dispose();

        protected override Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken)
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            return Task.FromResult<Stream>(new MemoryStreamMock(_memoryStream));
        }

        protected override Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken)
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            _memoryStream.SetLength(0);

            return Task.FromResult<Stream>(new MemoryStreamMock(_memoryStream));
        }
    }
}