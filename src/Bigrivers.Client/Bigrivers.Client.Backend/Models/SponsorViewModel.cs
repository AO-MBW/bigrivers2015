using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class SponsorViewModel
    {
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Display(Name = "Website")]
        public string Url { get; set; }

        public bool Status { get; set; }
    }
}