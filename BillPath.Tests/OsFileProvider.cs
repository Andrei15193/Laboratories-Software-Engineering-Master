using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;

namespace BillPath.Tests
{
    public sealed class OsFileProvider
        : FileProvider
    {
        public override Task<bool> FileExistsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(File.Exists(FileName));
        }

        public override Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public override Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
        }
    }
}