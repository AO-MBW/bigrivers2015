using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Principal;

namespace Bigrivers.Server.Model
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string YoutubeChannel { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual List<Performance> Performances { get; set; }
        public virtual List<Genre> Genres { get; set; }
        public virtual File Avatar { get; set; }
    }
}