using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Bigrivers.Server.Data;

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
                return BigriversDb.Create().Roles.Select(s => new SelectListItem
                {
                    Value = s.Name,
                    Text = s.Name
                }).ToList();
            }
        }
    }
}