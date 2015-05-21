using System.ComponentModel.DataAnnotations;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class SponsorViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Afbeelding")]
        public File Image { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string Url { get; set; }

        public bool Status { get; set; }
    }
}