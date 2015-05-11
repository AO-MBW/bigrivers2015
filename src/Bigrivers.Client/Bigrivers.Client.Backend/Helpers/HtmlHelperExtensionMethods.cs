using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Bigrivers.Client.Backend.Helpers
{
    public static class HtmlHelperExtensionMethods
    {
        private static CloudStorageAccount storageAccount;
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer container;

        static HtmlHelperExtensionMethods()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference("files");
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
        }

        public static MvcHtmlString HyperLink(this HtmlHelper helper, string text, string href)
        {
            return HyperLink(helper, text, href, false);
        }

        public static MvcHtmlString HyperLink(this HtmlHelper helper, string text, string href, bool openInNewWindow)
        {
            return HyperLink(helper, text, href, openInNewWindow, null);
        }

        public static MvcHtmlString HyperLink(this HtmlHelper helper, string text, string href, bool openInNewWindow, object htmlAttributes)
        {
            return HyperLink(helper, text, href, openInNewWindow, htmlAttributes, null);
        }

        public static MvcHtmlString HyperLink(this HtmlHelper helper, string text, string href, bool openInNewWindow, object htmlAttributes, object dataAttributes)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            // Data attributes are definitely a nice to have.
            // I don't know of a better way of rendering them using the RouteValueDictionary however.
            if (dataAttributes != null)
            {
                var values = new RouteValueDictionary(dataAttributes);

                foreach (var value in values)
                {
                    builder.MergeAttribute("data-" + value.Key, value.Value.ToString());
                }
            }

            // Returns proper link to itself if link left empty
            if (href == null) href = "#";

            builder.MergeAttribute("href", href);
            if (openInNewWindow) builder.MergeAttribute("target", "_blank");

            builder.SetInnerText(text);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ShowImage(this HtmlHelper helper, string key, string addClass = "", string addAttribute = "")
        {
            var image = container.GetBlockBlobReference(key).Uri.ToString();

            var validUrl = false;
            try
            {
                new Uri(image);
                validUrl = true;
            }
            catch { }

            if (!validUrl)
            {
                return new MvcHtmlString("<span class='error'>Make sure the string is an available URL</span>");
            }

            if (addClass != "")
            {
                addClass = string.Format("class='{0}'", addClass);
            }
            var showImage = string.Format("<img src='{0}' {1} {2}>", image, addClass, addAttribute);
            return new MvcHtmlString(showImage);
        }
    }
}