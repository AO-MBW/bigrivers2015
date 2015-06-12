using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class ArtistViewModel
    {
        [Required(ErrorMessage = "Het veld Gebruikersnaam is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [AllowHtml]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Upload een afbeelding voor de artiest")]
        public FileUploadViewModel Image { get; set; }

        [Display(Name = "Youtubekanaal")]
        [DataType(DataType.Url)]
        public string YoutubeChannel { get; set; }

        [Display(Name = "Website")]
        [DataType(DataType.Url)]
        public string Website { get; set; }

        [Display(Name = "Facebookpagina")]
        [DataType(DataType.Url)]
        public string Facebook { get; set; }

        [Display(Name = "Twitterpagina")]
        [DataType(DataType.Url)]
        public string Twitter { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}