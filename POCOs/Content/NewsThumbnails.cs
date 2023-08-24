using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StellarStreamAPI.POCOs.Content
{
    public class NewsThumbnails
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string NewsSite { get; set; }
        public string Summary { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
