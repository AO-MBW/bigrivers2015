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

        /// <summary>
        /// Returns the Url of the given File object
        /// </summary>
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

        /// <summary>
        /// Checks size in Bytes of the given file.
        /// Returns false if the file exceeds the maximum size.
        /// </summary>
        public static bool IsSize(HttpPostedFileBase file, int maxSize)
        {
            return file.ContentLength <= maxSize;
        }

        /// <summary>
        /// Checks size in Bytes or byte multiples (KB, MB, GB) of the given file.
        /// byteMultiplier must be lowercase kb, mb or gb.
        /// Returns false if the file exceeds the maximum size.
        /// </summary>
        public static bool IsSize(HttpPostedFileBase file, int maxSize, string byteMultiplier)
        {
            switch (byteMultiplier)
            {
                case "kb":
                    maxSize *= 1000;
                    break;
                case "mb":
                    maxSize *= 1000000;
                    break;
                case "gb":
                    maxSize *= 1000000000;
                    break;
            }
            return IsSize(file, maxSize);
        }

        /// <summary>
        /// Checks if given file is of any MIME type given in array.
        /// Returns false if it doesn't match with any mime type in array.
        /// </summary>
        public static bool IsMimes(HttpPostedFileBase file, string[] mimeTypes)
        {
            return !mimeTypes.Any() || mimeTypes.All(mime => file.ContentType.Contains(mime));
        }

        /// <summary>
        /// Uploads a file to AzureStorage into given container.
        /// Returns the File object generated from the given file.
        /// Will not upload file if it already exists in given container, and return the File object corresponding to existing file instead.
        /// </summary>
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

        /// <summary>
        /// Checks if file is smaller than given dimensions.
        /// Returns false if the file is larger than given dimensions.
        /// </summary>
        public static bool IsSmallerThanDimensions(Stream imageStream, int maxHeight, int maxWidth)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width < maxWidth && img.Height < maxHeight) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if file is exactly the given dimensions.
        /// Returns false if the file is not of the given dimensions.
        /// </summary>
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