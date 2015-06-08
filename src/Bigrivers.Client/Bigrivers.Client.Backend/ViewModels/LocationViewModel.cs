using System.ComponentModel.DataAnnotations;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class LocationViewModel
    {
        [Required]
        [Display(Name = "Podium naam")]
        public string Stagename { get; set; }
        [Required]
        [Display(Name = "Straat")]
        public string Street { get; set; }
        [Display(Name="Huisnummer")]
        public string Number { get; set; }
        [Required]
        [Display(Name = "Postcode")]
        public string Zipcode { get; set; }
        [Required]
        [Display(Name = "Stad")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Zichtbaar")]
        public bool Status { get; set; }
    }
}