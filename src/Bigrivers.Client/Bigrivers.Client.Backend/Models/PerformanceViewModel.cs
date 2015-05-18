using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PerformanceViewModel
    {
        [Required]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Starttijd")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Eindtijd")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public bool Status { get; set; }

        [Required]
        [Display(Name = "Artiest")]
        public int? Artist { get; set; }

        [Required]
        [Display(Name = "Evenement")]
        public int? Event { get; set; }

        public IEnumerable<SelectListItem> Events { get; set; }
        public IEnumerable<SelectListItem> Artists { get; set; }
    }
}