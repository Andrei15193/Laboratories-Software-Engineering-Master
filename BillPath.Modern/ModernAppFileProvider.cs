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
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
        }

        public override async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken)
        {
            return (await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName)) as IStorageFile != null;
        }

        public override async Task<Stream> GetReadStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            var storageFile = await ApplicationData
                .Current
                .LocalFolder
                .GetFileAsync(fileName)
                .AsTask(cancellationToken);
            return await storageFile.OpenStreamForReadAsync();
        }

        public override async Task<Stream> GetWriteStreamForAsync(string fileName, CancellationToken cancellationToken)
        {
            var storageFile = await ApplicationData
                .Current
                .LocalFolder
                .CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting)
                .AsTask(cancellationToken);
            return await storageFile.OpenStreamForWriteAsync();
        }
    }
}