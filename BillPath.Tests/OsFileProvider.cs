using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;

namespace BillPath.Tests
{
    public sealed class OsFileProvider
        : FileProvider
    {
        public override Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken)
        {
            return Task.FromResult(File.Exists(fileName));
        }

        public override Task<Stream> GetReadStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public override Task<Stream> GetWriteStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
        }
    }
}