using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigrivers.Server.Model
{
    public class Link
    {
        public int Id { get; set; }
        public string Logo { get; set; }

        [DefaultValue(true)]
        public bool YoutubeChannelStatus { get; set; }
        public string YoutubeChannel { get; set; }

        [DefaultValue(true)]
        public bool WebsiteStatus { get; set; }
        public string Website { get; set; }

        [DefaultValue(true)]
        public bool FacebookStatus { get; set; }
        public string Facebook { get; set; }

        [DefaultValue(true)]
        public bool TwitterStatus { get; set; }
        public string Twitter { get; set; }
    }
}
