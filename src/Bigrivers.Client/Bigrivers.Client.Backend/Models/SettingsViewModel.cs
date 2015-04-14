using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class SettingsViewModel
    {
        [Display(Name = "Bigrivers Youtubekanaal")]
        [DataType(DataType.Url)]
        public string YoutubeChannel { get; set; }

        [Display(Name = "Bigrivers Facebookpagina")]
        [DataType(DataType.Url)]
        public string Facebook { get; set; }

        [Display(Name = "Bigrivers Twitterpagina")]
        [DataType(DataType.Url)]
        public string Twitter { get; set; }
    }
}