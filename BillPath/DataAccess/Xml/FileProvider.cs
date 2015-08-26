using System.IO;
using System.Threading.Tasks;

namespace BillPath.DataAccess.Xml
{
    public abstract class FileProvider
    {
        public abstract Task<bool> FileExistsAsync();
        public abstract Task<Stream> GetReadStreamAsync();
        public abstract Task<Stream> GetWriteStreamAsync();

        public string FileName
        {
            get;
            set;
        }
    }
}