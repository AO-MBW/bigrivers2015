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
        [Display(Name = "Op evenement")]
        public int? Location { get; set; }

        [Required]
        public bool Status { get; set; }

        public IEnumerable<SelectListItem> Locations
        {
            get
            {
                return _db.Locations
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Stagename
                    })
                    .ToList();
            }
        }
    }
}