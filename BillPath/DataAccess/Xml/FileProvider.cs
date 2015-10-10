using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess.Xml
{
    public abstract class FileProvider
    {
        public Task<bool> FileExistsAsync(string fileName)
            => FileExistsAsync(fileName, CancellationToken.None);
        public abstract Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken);

        public Task<Stream> GetReadStreamForAsync(string fileName)
            => GetReadStreamForAsync(fileName, CancellationToken.None);
        public abstract Task<Stream> GetReadStreamForAsync(string fileName, CancellationToken cancellationToken);

        public Task<Stream> GetWriteStreamForAsync(string fileName)
            => GetWriteStreamForAsync(fileName, CancellationToken.None);
        public abstract Task<Stream> GetWriteStreamForAsync(string fileName, CancellationToken cancellationToken);

        public Task DeleteFileAsync(string fileName)
            => DeleteFileAsync(fileName, CancellationToken.None);
        public abstract Task DeleteFileAsync(string fileName, CancellationToken cancellationToken);
    }
}