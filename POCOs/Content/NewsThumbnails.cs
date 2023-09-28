using StellarStreamAPI.POCOs.Content.ThumbnailSubmodels;

namespace StellarStreamAPI.POCOs.Content
{
    public class NewsThumbnails
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string NewsSite { get; set; }
        public string Summary { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Featured { get; set; }
        public List<Launches> Launches { get; set; } = new List<Launches>();
    }
}
