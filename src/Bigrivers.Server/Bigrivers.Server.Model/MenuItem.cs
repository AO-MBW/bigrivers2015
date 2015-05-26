using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int? Order { get; set; }
        public int? Parent { get; set; }
        public bool IsParent { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Link Target { get; set; }
     }
}