using System;
using System.Collections.Generic;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.Models
{
    public class PerformanceListViewModel
    {
        public DateTime Date { get; set; }
        public virtual List<Performance> Performances { get; set; } 
    }
}