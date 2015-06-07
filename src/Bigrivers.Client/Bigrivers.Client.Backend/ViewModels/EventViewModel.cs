using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Data;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class EventViewModel
    {
        private readonly BigriversDb _db = new BigriversDb(); 
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        public bool YoutubeChannelStatus { get; set; }
        public bool FacebookStatus { get; set; }
        public bool TwitterStatus { get; set; }

        [Required]
        [Display(Name = "Starttijd")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Eindtijd")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        [Display(Name = "Entreeprijs")]
        public decimal? Price { get; set; }
        public bool TicketRequired { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}