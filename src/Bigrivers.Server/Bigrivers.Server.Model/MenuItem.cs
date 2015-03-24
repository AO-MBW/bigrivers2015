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
        public int Order { get; set; }
        public int Parent { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }
        public MenuItemType MenuItemType { get; set; }
        public string Content { get; set; }

        // ExternURL:   "http://www.crowdfunding.com/bigrivers"
        // FileURL:     "/files/programma.pdf" ;  dit wijst naar een plek op de bigrivers webserver waar alle downloadbare files staan
        // Page:        "http://www.bigrivers.nl/page</id>" => "[event(2)] <br> Dit is een mooi evenement. De volgende artiesten komen daar: <br> [event(2)\artists]"
    }
}