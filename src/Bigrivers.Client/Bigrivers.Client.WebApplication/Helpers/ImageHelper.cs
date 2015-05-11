using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using File = Bigrivers.Server.Model.File;

namespace Bigrivers.Client.WebApplication.Helpers
{
    public static class ImageHelpers
    {
        private static readonly CloudBlobContainer Container;

        static ImageHelpers()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();

            Container = blobClient.GetContainerReference("files");
            Container.CreateIfNotExists();
            Container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
        }

        public static string GetImageUrl(string key)
        {
            return Container.GetBlockBlobReference(key).Uri.ToString();
        }

        public static File UploadFile(HttpPostedFileBase file, int maxSize, string[] mimeTypes)
        {
            if (mimeTypes.Any())
            {
                if (mimeTypes.Any(mime => !file.ContentType.Contains(mime)))
                {
                    throw new Exception("File is not of required type");
                }
            }
            if (file.ContentLength > maxSize) throw new Exception("File too large");

            var extension = Path.GetExtension(file.FileName);
            var key = string.Format("File-{0}{1}", Guid.NewGuid(), extension);

            var photoEntity = new File
            {
                Name = file.FileName.Replace(extension, ""),
                ContentLength = file.ContentLength,
                ContentType = file.ContentType,
                Key = key
            };

            var block = Container.GetBlockBlobReference(key);
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