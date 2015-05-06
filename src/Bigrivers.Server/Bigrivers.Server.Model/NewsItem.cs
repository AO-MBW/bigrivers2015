using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}