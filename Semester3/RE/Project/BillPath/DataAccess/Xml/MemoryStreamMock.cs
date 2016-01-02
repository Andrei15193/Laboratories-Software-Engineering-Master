using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess.Xml
{
    internal sealed class MemoryStreamMock
        : Stream
    {
        private readonly MemoryStream _memoryStream;

        public MemoryStreamMock(MemoryStream memoryStream)
        {
            _memoryStream = memoryStream;
        }

        public override bool CanRead
            => _memoryStream.CanRead;

        public override bool CanWrite
            => _memoryStream.CanWrite;

        public override bool CanSeek
            => _memoryStream.CanSeek;

        public override long Length
            => _memoryStream.Length;

        public override long Position
        {
            get
            {
                return _memoryStream.Position;
            }
            set
            {
                _memoryStream.Position = value;
            }
        }

        public override bool CanTimeout
            => _memoryStream.CanTimeout;
        public override int ReadTimeout
        {
            get
            {
                return _memoryStream.ReadTimeout;
            }
            set
            {
                _memoryStream.ReadTimeout = value;
            }
        }
        public override int WriteTimeout
        {
            get
            {
                return _memoryStream.WriteTimeout;
            }
            set
            {
                _memoryStream.WriteTimeout = value;
            }
        }

        public override void Flush()
            => _memoryStream.Flush();
        public override Task FlushAsync(CancellationToken cancellationToken)
            => _memoryStream.FlushAsync(cancellationToken);

        public override int ReadByte()
            => _memoryStream.ReadByte();
        public override int Read(byte[] buffer, int offset, int count)
            => _memoryStream.Read(buffer, offset, count);
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => _memoryStream.ReadAsync(buffer, offset, count, cancellationToken);

        public override void WriteByte(byte value)
            => _memoryStream.WriteByte(value);
        public override void Write(byte[] buffer, int offset, int count)
            => _memoryStream.Write(buffer, offset, count);
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => _memoryStream.WriteAsync(buffer, offset, count, cancellationToken);

        public override long Seek(long offset, SeekOrigin origin)
            => _memoryStream.Seek(offset, origin);
        public override void SetLength(long value)
            => _memoryStream.SetLength(value);

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            => _memoryStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }
}