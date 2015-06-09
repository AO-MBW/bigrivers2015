using System;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Performance
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Event Event { get; set; }
        public virtual Location Location { get; set; }
    }
}