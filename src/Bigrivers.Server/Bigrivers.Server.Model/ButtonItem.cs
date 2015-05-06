using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigrivers.Server.Model
{
    public class ButtonItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string URL { get; set; }
        public string Logo { get; set; }
        public int? Order { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
