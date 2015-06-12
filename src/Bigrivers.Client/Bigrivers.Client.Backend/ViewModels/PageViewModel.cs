using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PageViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}