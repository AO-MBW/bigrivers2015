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
        public string EditorContent { get; set; }

        [Display(Name = "Inhoud")]
        public string IFrame { get; set; }

        [AllowHtml]
        [Display(Name = "Inhoud")]
        public string HtmlContent { get; set; }

        public bool Status { get; set; }
    }
}