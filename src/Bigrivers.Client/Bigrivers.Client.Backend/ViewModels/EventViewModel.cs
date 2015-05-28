using System;
using System.ComponentModel.DataAnnotations;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class EventViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Korte beschrijving")]
        public string ShortDescription { get; set; }

        public bool WebsiteStatus { get; set; }
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