using System.Linq;
using Bigrivers.Server.Model;

namespace Bigrivers.Client.WebApplication.ViewModels
{
    public class SettingsViewModel
    {
        public string YoutubeChannel { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public string Hashtag
        {
            get { return Twitter.Split('/').Last(); }
        }

        public File Image { get; set; }
    }
}