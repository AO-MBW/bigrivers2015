using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool YoutubeChannelStatus { get; set; }
        [DefaultValue(true)]
        public bool FacebookStatus { get; set; }
        [DefaultValue(true)]
        public bool TwitterStatus { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public decimal Price { get; set; }
        public bool TicketRequired { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual List<Performance> Performances { get; set; }
        public virtual List<Sponsor> Sponsors { get; set; }
    }
}