using System;
using System.ComponentModel;
using System.Security.Principal;

namespace Bigrivers.Server.Model
{
    public class WidgetItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? Order { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Link Target { get; set; }
        public virtual File Image { get; set; }
    }
}
