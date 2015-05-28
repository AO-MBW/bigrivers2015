using System.Web;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Client.Helpers;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.Helpers
{
    public class LinkManageHelper
    {
        private static readonly BigriversDb Db = new BigriversDb();

        public static Link SetLink(LinkViewModel viewModel)
        {
            return SetLink(viewModel, null);
        }

        public static Link SetLink(LinkViewModel viewModel, HttpPostedFileBase file)
        {
            var link = new Link
            {
                Type = viewModel.LinkType
            };
            // Set the url variable to a working url based on the link type
            switch (viewModel.LinkType)
            {
                case "internal":
                    link.InternalType = viewModel.InternalType;
                    // Get the correct 'Internal Id' from the form
                    switch (viewModel.InternalType)
                    {
                        case "Events":
                            link.InternalId = viewModel.InternalEventId;
                            break;
                        case "Artists":
                            link.InternalId = viewModel.InternalArtistId;
                            break;
                        case "Performances":
                            link.InternalId = viewModel.InternalPerformanceId;
                            break;
                        case "News":
                            link.InternalId = viewModel.InternalNewsId;
                            break;
                        case "Sponsors":
                            link.InternalId = viewModel.InternalSponsorId;
                            break;
                        case "Contact":
                            break;
                    }
                    break;
                case "external":
                    // Make sure the URL uses Http if it doesn't use Http / Https already
                    if (!viewModel.ExternalUrl.StartsWith("http")) viewModel.ExternalUrl = string.Format("http://{0}", viewModel.ExternalUrl);
                    link.ExternalUrl = viewModel.ExternalUrl;
                    break;
                case "file":
                    link.File = FileUploadHelper.UploadFile(file, "uploadedfiles");
                    break;
            }
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
                case "Artists":
                    viewModel.InternalArtistId = link.InternalId;
                    break;
                case "Performances":
                    viewModel.InternalPerformanceId = link.InternalId;
                    break;
                case "News":
                    viewModel.InternalNewsId = link.InternalId;
                    break;
                case "Sponsors":
                    viewModel.InternalSponsorId = link.InternalId;
                    break;
            }

            return viewModel;
        }
    }
}