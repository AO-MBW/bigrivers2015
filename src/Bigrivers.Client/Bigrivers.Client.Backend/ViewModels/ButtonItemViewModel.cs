using System.ComponentModel.DataAnnotations;
using Bigrivers.Client.Backend.Models;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class ButtonItemViewModel
    {
        [Required(ErrorMessage = "Het veld Weergavenaam is verplicht")]
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }
        public LinkViewModel LinkView { get; set; }
        [Display(Name = "Button afbeelding")]
        public FileUploadViewModel Image { get; set; }
        public int? Order { get; set; }
        public bool Status { get; set; }
    }
}