using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bigrivers.Client.WebApplication.ViewModels
{
    public class SettingsViewModel
    {
        public string YoutubeChannel { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Hashtag { get; set; }
    }
}