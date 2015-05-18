using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bigrivers.Client.Helpers
{
    public static class HtmlHelperExtensionMethods
    {
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

        public static MvcHtmlString ShowImage(this HtmlHelper helper, string url)
        {
            return ShowImage(helper, url, null, null);
        }

        public static MvcHtmlString ShowImage(this HtmlHelper helper, string url, object htmlAttributes)
        {
            return ShowImage(helper, url, htmlAttributes, null);
        }

        public static MvcHtmlString ShowImage(this HtmlHelper helper, string url, object htmlAttributes, object dataAttributes)
        {
            var validUrl = false;

            var builder = new TagBuilder("img");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

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

            builder.MergeAttribute("src", url);

            try
            {
                new Uri(url);
                validUrl = true;
            }
            catch { }

            if (!validUrl)
            {
                return new MvcHtmlString("<span class='error'>Make sure the string is an available URL</span>");
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}