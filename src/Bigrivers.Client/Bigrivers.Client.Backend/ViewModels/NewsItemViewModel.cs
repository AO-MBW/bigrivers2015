using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class NewsItemViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Afbeelding")]
        public FileUploadViewModel Image { get; set; }

        public bool Status { get; set; }
    }
}