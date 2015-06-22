using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Helpers
{
    public static class LinkHelper
    {
        private static readonly BigriversDb Db = new BigriversDb();

        public static string GetUrl(Link link)
        {
            return GetUrl(link, false);
        }

        /// <summary>
        /// Returns the URL of the given Link object
        /// </summary>
        public static string GetUrl(Link link, bool readableUrl)
        {
            switch (link.Type)
            {
                case "internal":
                    return string.Format("/Home/{0}/{1}", link.InternalType, link.InternalId);
                case "external":
                    return link.ExternalUrl;
                case "file":
                    return readableUrl ? "Bestand: " + link.File.Name : ImageHelper.GetImageUrl(link.File);
            }
            return "";
        }
    }
}
