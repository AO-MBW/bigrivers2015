using System.Web.Mvc;
using Bigrivers.Server.Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Bigrivers.Client.Backend.Controllers
{
    public class BaseController : Controller
    {
        protected readonly BigriversDb Db = new BigriversDb();
        protected CloudStorageAccount StorageAccount;
        protected CloudBlobClient BlobClient;
        protected CloudBlobContainer Container;

        public BaseController()
        {
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }
    }
}