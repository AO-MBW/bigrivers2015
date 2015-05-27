using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PerformanceViewModel
    {
        [Required(ErrorMessage = "Het veld beschrijving is niet ingevuld")]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Starttijd")]
        public DateTime Start { get; set; }

        [Display(Name = "Eindtijd")]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "Door artiest")]
        public int? Artist { get; set; }

        [Required]
        [Display(Name = "Op evenement")]
        public int? Event { get; set; }

        public bool Status { get; set; }

        public IEnumerable<SelectListItem> Events { get; set; }
        public IEnumerable<SelectListItem> Artists { get; set; }
    }
}