using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class EventViewModel
    {
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Korte beschrijving")]
        public string ShortDescription { get; set; }

        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string FrontpageLogo { get; set; }

        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        public string BackgroundImage { get; set; }

        [Display(Name = "Starttijd")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Display(Name = "Eindtijd")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public decimal? Price { get; set; }
        [Display(Name = "Zijn tickets verplicht?")]
        public bool TicketRequired { get; set; }

        public bool Status { get; set; }
    }
}