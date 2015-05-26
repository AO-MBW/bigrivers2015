namespace Bigrivers.Server.Model
{
    public class Link
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ExternalUrl { get; set; }
        public string InternalType { get; set; }
        public string InternalId { get; set; }
        public virtual File File { get; set; }
    }
}