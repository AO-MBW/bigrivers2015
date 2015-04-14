using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(true)]
        public bool Deleted { get; set; }
    }
}