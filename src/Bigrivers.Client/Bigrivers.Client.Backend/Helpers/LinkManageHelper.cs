using System.Linq;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Helpers
{
    public class LinkManageHelper
    {
        private static readonly BigriversDb Db = new BigriversDb();

        public static Link SetLink(LinkViewModel model)
        {
            var link = new Link
            {
                Type = model.LinkType
            };
            // Set the url variable to a working url based on the link type
            switch (model.LinkType)
            {
                case "internal":
                    link.InternalType = model.InternalType;
                    // Get the correct 'Internal Id' from the form
                    switch (model.InternalType)
                    {
                        case "Events":
                            link.InternalId = model.InternalEventId;
                            break;
                        case "Performances":
                            link.InternalId = model.InternalPerformanceId;
                            break;
                        case "Artists":
                            link.InternalId = model.InternalArtistId;
                            break;
                        case "Page":
                            link.InternalId = model.InternalPageId;
                            break;
                        case "News":
                            link.InternalId = model.InternalNewsId;
                            break;
                    }
                    break;
                case "external":
                    // Make sure the URL uses Http if it doesn't use Http / Https already
                    if (!model.ExternalUrl.StartsWith("http")) model.ExternalUrl = string.Format("http://{0}", model.ExternalUrl);
                    link.ExternalUrl = model.ExternalUrl;
                    break;
                case "file":
                    if (model.File.UploadFile != null || model.File.Key != null && model.File.Key != "false")
                    {
                        File fileEntity = null;
                        // Either upload file to AzureStorage or use file Key from explorer to get the file
                        if (model.File.NewUpload)
                        {
                            if (model.File.UploadFile != null)
                            {
                                fileEntity = FileUploadHelper.UploadFile(model.File.UploadFile, FileUploadLocation.LinkUpload);
                            }
                        }
                        else
                        {
                            if (model.File.Key != "false")
                            {
                                fileEntity = Db.Files.Single(m => m.Key == model.File.Key);
                            }
                        }

                        link.File = Db.Files.SingleOrDefault(m => m.Key == fileEntity.Key);
                    }
                    break;
            }
            Db.Links.Add(link);
            Db.SaveChanges();
            return link;
        }

        public static LinkViewModel SetViewModel(Link link, LinkViewModel viewModel)
        {
            viewModel.LinkType = link.Type;
            viewModel.InternalType = link.InternalType ?? "Events";
            viewModel.ExternalUrl = link.ExternalUrl;

            // Set the Id for the 'Internal Id'
            switch (link.InternalType)
            {
                case "Events":
                    viewModel.InternalEventId = link.InternalId;
                    break;
                case "Performances":
                    viewModel.InternalPerformanceId = link.InternalId;
                    break;
                case "Artists":
                    viewModel.InternalArtistId = link.InternalId;
                    break;
                case "Page":
                    viewModel.InternalPageId = link.InternalId;
                    break;
                case "News":
                    viewModel.InternalNewsId = link.InternalId;
                    break;
            }

            return viewModel;
        }
    }
}