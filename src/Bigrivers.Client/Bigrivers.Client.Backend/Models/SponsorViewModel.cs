using System.ComponentModel.DataAnnotations;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class SponsorViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string Url { get; set; }

        public bool Status { get; set; }
    }
}