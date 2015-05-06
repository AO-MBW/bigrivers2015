using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.Backend.ViewModels
{
    public class MenuItemViewModel
    {
        [Display(Name = "URL")]
        [DataType(DataType.Url)]
        public string URL { get; set; }

        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }

        public int? Order { get; set; }
        public int Parent { get; set; }

        public bool Status { get; set; }
    }
}