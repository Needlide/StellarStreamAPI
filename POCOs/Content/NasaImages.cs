using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Content
{
    public class NasaImages
    {
        [BsonElement("center")]
        public string Center { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("nasa_id")]
        public string NasaId { get; set; }

        [BsonElement("media_type")]
        public string MediaType { get; set; }

        [BsonElement("keywords")]
        public List<string> Keywords { get; set; }

        [BsonElement("date_created")]
        public DateTime DateCreated { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("album")]
        public List<string> Album { get; set; }
    }
}
