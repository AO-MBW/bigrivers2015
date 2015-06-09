using System;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public enum ButtonType { Regular = 1, SponsorWidget, NewsWidget }
    public class ButtonItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? Order { get; set; }
        [DefaultValue(ButtonType.Regular)]
        public ButtonType Type { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Link Target { get; set; }
        public virtual File Logo { get; set; }
    }
}
