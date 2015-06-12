using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Data;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PerformanceViewModel
    {
        private readonly BigriversDb _db = new BigriversDb();

        [AllowHtml]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }
        [Display(Name = "Starttijd")]
        public DateTime Start { get; set; }
        [Display(Name = "Eindtijd")]
        public DateTime End { get; set; }

        public bool Status { get; set; }

        [Required]
        [Display(Name = "Door artiest")]
        public int? Artist { get; set; }
        [Required]
        [Display(Name = "Op evenement")]
        public int? Event { get; set; }
        [Required]
        [Display(Name = "Op podium")]
        public int? Location { get; set; }

        public IEnumerable<SelectListItem> Events
        {
            get
            {
                return _db.Events
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Title
                    })
                    .ToList();
            }
        }
        public IEnumerable<SelectListItem> Artists
        {
            get
            {
                return _db.Artists
                    .Where(m => !m.Deleted)
                    .ToList()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToList();
            }
        }
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