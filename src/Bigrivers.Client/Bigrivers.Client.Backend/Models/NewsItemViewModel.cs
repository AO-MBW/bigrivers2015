using System.ComponentModel.DataAnnotations;

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

        [Required]
        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        public bool Status { get; set; }
    }
}