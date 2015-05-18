using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class NewsItemViewModel
    {
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        public bool Status { get; set; }
    }
}