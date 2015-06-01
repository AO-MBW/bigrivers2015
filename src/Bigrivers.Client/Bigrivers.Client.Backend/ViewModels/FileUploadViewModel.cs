using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class FileUploadViewModel
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string Key { get; set; }
        public bool NewUpload { get; set; }
        public List<File> FileBase { get; set; }

        public virtual File ExistingFile { get; set; }
    }
}