using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class ArtistViewModel
    {
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string Avatar { get; set; }

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

        public bool Status { get; set; }
    }
}