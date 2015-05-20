using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Url)]
        public string URL { get; set; }

        public File Logo { get; set; }

        public int? Order { get; set; }

        public bool Status { get; set; }
    }
}