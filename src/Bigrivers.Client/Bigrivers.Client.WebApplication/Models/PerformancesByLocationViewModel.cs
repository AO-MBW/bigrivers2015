using System.Collections.Generic;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.Models
{
    public class PerformancesByLocationViewModel
    {
        public Location Stage { get; set; }
        public List<Performance> Performances { get; set; }
    }
}