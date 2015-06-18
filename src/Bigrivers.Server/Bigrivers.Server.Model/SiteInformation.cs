namespace Bigrivers.Server.Model
{
    public class SiteInformation
    {
        public int Id { get; set; }
        public string YoutubeChannel { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Date { get; set; }

        public virtual File Image { get; set; }
    }
}
