﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class SettingsViewModel
    {
        [Required]
        [Display(Name = "Bigrivers Youtubekanaal")]
        [DataType(DataType.Url)]
        public string YoutubeChannel { get; set; }

        [Required]
        [Display(Name = "Bigrivers Facebookpagina")]
        [DataType(DataType.Url)]
        public string Facebook { get; set; }

        [Required]
        [Display(Name = "Bigrivers Twitterpagina")]
        [DataType(DataType.Url)]
        public string Twitter { get; set; }

        [Display(Name = "Datum in Logo")]
        public string Date { get; set; }

        public FileUploadViewModel Image { get; set; }

    }
}