using System;
using System.Collections.Generic;

namespace Bigrivers.Client.WebApplication.Models
{
    public class StagesViewModel
    {
        public DateTime Date { get; set; }
        public List<PerformancesByLocationViewModel> Stages { get; set; } 
    }
}