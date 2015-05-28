using System.ComponentModel.DataAnnotations;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class NewsItemViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Afbeelding")]
        public File Image { get; set; }

        public bool Status { get; set; }
    }
}