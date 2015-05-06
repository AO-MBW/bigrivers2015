using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigrivers.Server.Model
{
    public enum MenuItemType { ExternURL, FileURL, Page };

    public class MenuItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string URL { get; set; }
        public int? Order { get; set; }
        public int Parent { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
     }
}