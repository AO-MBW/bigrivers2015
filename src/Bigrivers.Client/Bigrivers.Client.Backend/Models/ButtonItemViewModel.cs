using System.ComponentModel.DataAnnotations;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class ButtonItemViewModel
    {
        [Display(Name = "URL")]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        public File Logo { get; set; }

        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }

        public int? Order { get; set; }

        public bool Status { get; set; }
    }
}