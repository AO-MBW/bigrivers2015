using System.ComponentModel.DataAnnotations;
using Bigrivers.Client.Backend.Models;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class ButtonItemViewModel
    {
        [Required]
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }
        [Required]
        [Display(Name = "URL")]
        public LinkViewModel LinkView { get; set; }
        public File Logo { get; set; }
        public int? Order { get; set; }
        public bool Status { get; set; }
    }
}