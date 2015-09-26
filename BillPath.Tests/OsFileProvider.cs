using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using Windows.Storage;

namespace BillPath.Tests
{
    public sealed class OsFileProvider
        : FileProvider
    {
        public override async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken)
        {
            var file = await _GetStorageFileAsync(fileName, cancellationToken);
            return file.IsAvailable;
        }

        public override async Task<Stream> GetReadStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            var file = await _GetStorageFileAsync(fileName, cancellationToken);
            return await file.OpenStreamForReadAsync();
        }

        public override async Task<Stream> GetWriteStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            var storageFile = await _GetStorageFileAsync(fileName, cancellationToken);
            return await storageFile.OpenStreamForWriteAsync();
        }

        private static Task<StorageFile> _GetStorageFileAsync(string fileName, CancellationToken cancellationToken)
            => ApplicationData.Current.LocalFolder.GetFileAsync(fileName).AsTask(cancellationToken);
    }
}