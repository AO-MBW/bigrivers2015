using Bigrivers.Server.Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
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

            var serviceProperties = BlobClient.GetServiceProperties();

            var cors = new CorsRule();

            cors.AllowedOrigins.Add("*");
            //cors.AllowedOrigins.Add("mysite.com"); // more restrictive may be preferable
            cors.AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head;
            cors.MaxAgeInSeconds = 3600;

            serviceProperties.Cors.CorsRules.Add(cors);

            BlobClient.SetServiceProperties(serviceProperties);
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
    }
}