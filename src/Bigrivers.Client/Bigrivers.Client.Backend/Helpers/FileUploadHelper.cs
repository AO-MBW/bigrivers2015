using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Bigrivers.Client.Backend.ViewModels;
using Bigrivers.Server.Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using File = Bigrivers.Server.Model.File;

namespace Bigrivers.Client.Backend.Helpers
{
    public class FileUploadHelper
    {
        private static readonly BigriversDb Db = new BigriversDb();
        private static readonly CloudBlobClient BlobClient;
        private static CloudBlobContainer _container;
        
        static FileUploadHelper()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = storageAccount.CreateCloudBlobClient();
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
            file.InputStream.Position = 0;
            var extension = Path.GetExtension(file.FileName);
            var key = string.Format("File-{0}{1}", Guid.NewGuid(), extension);

            var hash = new MD5Cng().ComputeHash(file.InputStream);

            if (Db.Files.Any(m => m.Md5 == hash && m.Container == container))
            {
                return Db.Files.Single(m => m.Md5 == hash && m.Container == container);
            }

            var photoEntity = new File
            {
                Name = file.FileName.Replace(extension ?? "", ""),
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

            Db.Files.Add(photoEntity);
            Db.SaveChanges();
            var block = _container.GetBlockBlobReference(key);
            block.Properties.ContentType = file.ContentType;
            block.UploadFromStream(file.InputStream);
            return photoEntity;
        }
    }

    public static class ImageUploadHelper
    {
        /// <summary>
        /// Checks if image is smaller than given dimensions.
        /// Returns false if the image is larger than given dimensions.
        /// </summary>
        public static bool IsSmallerThanDimensions(Stream imageStream, int maxHeight, int maxWidth)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width <= maxWidth && img.Height <= maxHeight) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if image is larger than given dimensions.
        /// Returns false if the image is smaller than given dimensions.
        /// </summary>
        public static bool IsLargerThanDimensions(Stream imageStream, int minHeight, int minWidth)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width >= minWidth && img.Height >= minHeight) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if image is exactly the given dimensions.
        /// Returns false if the image is not of the given dimensions.
        /// </summary>
        public static bool IsExactDimensions(Stream imageStream, int height, int width)
        {
            using (var img = Image.FromStream(imageStream))
            {
                if (img.Width == width && img.Height == height) return true;
            }
            return false;
        }

        //public static bool IsAspectRatio(Stream imageStream, int height, int width)
        //{
        //    using (var img = Image.FromStream(imageStream))
        //    {
        //        if (img.Width == width && img.Height == height) return true;
        //    }
        //    return false;
        //}
    }

    public class FileUploadValidator
    {
        public FileUploadViewModel FileObject { get; set; }
        public FileUploadModelErrors ModelErrors { get; set; }
        public bool Required { get; set; }
        public int MaxByteSize { get; set; }
        public int? MaxHeight { get; set; }
        public int? MaxWidth { get; set; }
        public int? MinHeight { get; set; }
        public int? MinWidth { get; set; }
        public string[] MimeTypes { get; set; }

        public List<string> CheckFile(FileUploadViewModel file)
        {
            FileObject = file;
            return CheckFile();
        } 

        public List<string> CheckFile()
        {
            var modelErrorList = new List<string>();
            // Check if the file, if required, is actually supplied
            if (FileObject.ExistingFile == null && Required && (
                (FileObject.NewUpload && FileObject.UploadFile == null)
                || (!FileObject.NewUpload && 
                    (FileObject.Key == "false" || FileObject.Key == null))
                    ))
            {
                modelErrorList.Add(ModelErrors.Required);
            }
            else if (FileObject.UploadFile != null)
            {
                if (!FileUploadHelper.IsSize(FileObject.UploadFile, MaxByteSize))
                {
                    modelErrorList.Add(ModelErrors.ExceedsMaxByteSize);
                }
                if (!FileUploadHelper.IsMimes(FileObject.UploadFile, MimeTypes))
                {
                    modelErrorList.Add(ModelErrors.ForbiddenMime);
                }
                if (FileObject.UploadFile.ContentType.Contains("image"))
                {
                    if (!ImageUploadHelper.IsSmallerThanDimensions(FileObject.UploadFile.InputStream,
                    MaxHeight ?? 999999999,
                    MaxWidth ?? 999999999))
                    {
                        modelErrorList.Add(ModelErrors.ExceedsPixelSizeMaximum);
                    }
                    if (!ImageUploadHelper.IsLargerThanDimensions(FileObject.UploadFile.InputStream,
                        MaxHeight ?? 0,
                        MaxWidth ?? 0))
                    {
                        modelErrorList.Add(ModelErrors.BeneathPixelSizeMinimum);
                    }
                }
                
            }

            return modelErrorList;
        }
    }

    public class FileUploadModelErrors
    {
        public string Required { get; set; }
        public string ExceedsMaxByteSize { get; set; }
        public string ExceedsPixelSizeMaximum { get; set; }
        public string BeneathPixelSizeMinimum { get; set; }
        public string ForbiddenMime { get; set; }
    }

    public static class FileUploadLocation
    {
        public static string Artist { get { return "artist"; } }
        public static string Event { get { return "event"; } }
        public static string Performance { get { return "performance"; } }
        public static string News { get { return "news"; } }
        public static string NewsWidget { get { return "newswidget"; } }
        public static string ButtonLogo { get { return "buttonlogo"; } }
        public static string Sponsor { get { return "sponsor"; } }
        public static string LinkUpload { get { return "linkupload"; } }
    }
}