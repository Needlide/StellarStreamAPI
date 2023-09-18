using StellarStreamAPI.Abstraction;

namespace StellarStreamAPI.POCOs.Content
{
    public class Apod : DbEntityBase
    {
        public int Id { get; set; }
        public string Copyright { get; set; }
        public string Date { get; set; }
        public string Explanation { get; set; }
        public string HdUrl { get; set; }
        public string MediaType { get; set; }
        public string ServiceVersion { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
