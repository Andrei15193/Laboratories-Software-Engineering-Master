using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace BillPath.DataAccess.Xml
{
    public class ExpenseCategoryXmlFileRepository
        : ExpenseCategoryXmlRepository
    {
        private readonly string _fileName;

        public ExpenseCategoryXmlFileRepository(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                if (fileName == null)
                    throw new ArgumentNullException(nameof(fileName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(fileName));

            _fileName = fileName;
        }

        protected override async Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken)
        {
            var file = await ApplicationData
                .Current
                .LocalFolder
                .CreateFileAsync(
                    _fileName,
                    CreationCollisionOption.OpenIfExists)
                .AsTask(cancellationToken);
            return await file.OpenStreamForReadAsync();
        }

        protected override async Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken)
        {
            var file = await ApplicationData
                .Current
                .LocalFolder
                .CreateFileAsync(
                    _fileName,
                    CreationCollisionOption.ReplaceExisting)
                .AsTask(cancellationToken);
            return await file.OpenStreamForWriteAsync();
        }
    }
}