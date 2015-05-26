using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Data;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public IEnumerable<SelectListItem> Roles
        {
            get
            {
                return BigriversDb.Create().Roles.Where(m => m.Name != "developer").Select(s => new SelectListItem
                {
                    Value = s.Name,
                    Text = s.Name
                }).ToList();
            }
        }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Inlognaam")]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }

    public class UserViewModel : StaffMember
    {
        public string Role { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Herhaal nieuw wachtwoord")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord en herhaalde wachtwoord zijn niet gelijk")]
        public string ConfirmPassword { get; set; }
    }
}
