using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using Windows.Storage;

namespace BillPath.Modern
{
    public class ModernAppFileProvider
        : FileProvider
    {
        public ModernAppFileProvider()
        {
            Debug.WriteLine("Application path: " + ApplicationData.Current.LocalFolder.Path);
        }

        public override async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken)
            => ((await _GetStorageFileAsync(fileName, cancellationToken))?.IsAvailable) ?? false;

        public override async Task<Stream> GetReadStreamForAsync(string fileName, CancellationToken cancellationToken)
            => await (await _GetStorageFileAsync(fileName, cancellationToken)).OpenStreamForReadAsync();

        public override async Task<Stream> GetWriteStreamForAsync(string fileName, CancellationToken cancellationToken)
            => await (await _GetStorageFileAsync(fileName, cancellationToken)).OpenStreamForWriteAsync();

        public override async Task DeleteFileAsync(string fileName, CancellationToken cancellationToken)
            => await (await _GetStorageFileAsync(fileName, cancellationToken)).DeleteAsync().AsTask(cancellationToken);

        private static Task<StorageFile> _GetStorageFileAsync(string fileName, CancellationToken cancellationToken)
            => ApplicationData.Current.LocalFolder.GetFileAsync(fileName).AsTask(cancellationToken);
    }
}