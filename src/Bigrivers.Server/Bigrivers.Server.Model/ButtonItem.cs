using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class ButtonItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string URL { get; set; }
        public int? Order { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual File Logo { get; set; }
    }
}
