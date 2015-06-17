using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PageViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "IFrame URL")]
        public string IFrameLink { get; set; }

        [Display(Name = "IFrame Hoogte")]
        public int IFrameHeight { get; set; }

        [AllowHtml]
        [Display(Name = "HTML Content")]
        public string HtmlContent { get; set; }

        public bool Status { get; set; }
    }
}