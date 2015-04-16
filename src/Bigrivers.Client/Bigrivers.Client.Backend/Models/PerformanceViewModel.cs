using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class PerformanceViewModel
    {
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Starttijd")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Display(Name = "Eindtijd")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public bool Status { get; set; }
    }
}