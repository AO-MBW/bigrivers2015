using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Bigrivers.Server.Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using File = Bigrivers.Server.Model.File;

namespace Bigrivers.Client.Helpers
{
    public static class ImageHelper
    {
        private static readonly BigriversDb Db = new BigriversDb();
        private static readonly CloudBlobClient BlobClient;
        private static CloudBlobContainer _container;
        

        static ImageHelper()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = storageAccount.CreateCloudBlobClient();
        }

        public static string GetImageUrl(File file)
        {
            _container = BlobClient.GetContainerReference(file.Container);
            _container.CreateIfNotExists();
            _container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            return _container.GetBlockBlobReference(file.Key).Uri.ToString();
        }

        public static bool IsSize(HttpPostedFileBase file, int maxSize)
        {
            return file.ContentLength <= maxSize;
        }

        public static bool IsMimes(HttpPostedFileBase file, string[] mimeTypes)
        {
            return !mimeTypes.Any() || mimeTypes.All(mime => file.ContentType.Contains(mime));
        }

        public static File UploadFile(HttpPostedFileBase file, string container)
        {
            var extension = Path.GetExtension(file.FileName);
            var key = string.Format("File-{0}{1}", Guid.NewGuid(), extension);

            var hash = new MD5Cng().ComputeHash(file.InputStream);

            if (Db.Files.Any(m => m.Md5 == hash && m.Container == container))
            {
                return Db.Files.Single(m => m.Md5 == hash);
            }

            var photoEntity = new File
            {
                Name = file.FileName.Replace(extension, ""),
                ContentLength = file.ContentLength,
                ContentType = file.ContentType,
                Key = key,
                Container = container,
                Md5 = hash
            };

            file.InputStream.Position = 0;

            _container = BlobClient.GetContainerReference(container);
            _container.CreateIfNotExists();
            _container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            var block = _container.GetBlockBlobReference(key);
            block.Properties.ContentType = file.ContentType;
            block.UploadFromStream(file.InputStream);
            return photoEntity;
        }

        public static bool IsSmallerThanDimensions(Stream imageStream, int maxHeight, int maxWidth)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width < maxWidth && img.Height < maxHeight) return true;
            }
            return false;
        }

        public static bool IsExactDimensions(Stream imageStream, int height, int width)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width == width && img.Height == height) return true;
            }
            return false;
        }
    }
}