using System.Collections.Generic;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Artist> Artists { get; set; }

        [DefaultValue(true)]
        public bool Deleted { get; set; }
    }
}