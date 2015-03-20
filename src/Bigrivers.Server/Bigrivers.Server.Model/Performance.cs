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

        [DefaultValue(true)]
        public bool Status { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Event Event { get; set; }
    }
}