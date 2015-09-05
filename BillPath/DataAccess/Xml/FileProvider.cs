using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess.Xml
{
    public abstract class FileProvider
    {
        public Task<bool> FileExistsAsync()
        {
            return FileExistsAsync(CancellationToken.None);
        }
        public abstract Task<bool> FileExistsAsync(CancellationToken cancellationToken);

        public Task<Stream> GetReadStreamAsync()
        {
            return GetReadStreamAsync(CancellationToken.None);
        }
        public abstract Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken);

        public Task<Stream> GetWriteStreamAsync()
        {
            return GetWriteStreamAsync(CancellationToken.None);
        }
        public abstract Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken);

        public string FileName
        {
            get;
            set;
        }
    }
}