using System;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EditorContent { get; set; }
        public string IFrameLink { get; set; }
        public int IFrameHeight { get; set; }
        public string HtmlContent { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}