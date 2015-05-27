using System.ComponentModel.DataAnnotations;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class MenuItemViewModel
    {
        [Required]
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }
        public LinkViewModel LinkView { get; set; }
        public int? Order { get; set; }
        public int Parent { get; set; }

        public bool Status { get; set; }
    }
}