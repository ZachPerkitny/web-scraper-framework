namespace ScraperFramework.Data.Entities
{
    public class Endpoint
    {
        public int ID { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public int ProxyBlockID { get; set; }
    }
}
