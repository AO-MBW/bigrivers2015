using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;

namespace Bigrivers.Client.Backend.Helpers
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

        public static MvcHtmlString Header(this HtmlHelper helper, string level, string text)
        {
            var builder = new TagBuilder("h" + level);

            builder.SetInnerText(text);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Header(this HtmlHelper helper, string level, string text, object htmlAttributes)
        {
            var builder = new TagBuilder("h" + level);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            builder.SetInnerText(text);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Header(this HtmlHelper helper, string level, string text, object htmlAttributes, object dataAttributes)
        {
            var builder = new TagBuilder("h" + level);
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

            builder.SetInnerText(text);
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}