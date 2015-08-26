using System;
using System.Diagnostics;
using System.IO;
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

        public override async Task<bool> FileExistsAsync()
        {
            return (await ApplicationData.Current.LocalFolder.TryGetItemAsync(FileName)) as IStorageFile != null;
        }

        public override async Task<Stream> GetReadStreamAsync()
        {
            var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(FileName);
            return await storageFile.OpenStreamForReadAsync();
        }

        public override async Task<Stream> GetWriteStreamAsync()
        {
            var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            return await storageFile.OpenStreamForWriteAsync();
        }
    }
}