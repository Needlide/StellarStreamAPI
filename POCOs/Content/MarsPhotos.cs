using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StellarStreamAPI.POCOs.Content
{
    public class MarsPhotos
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public int Sol { get; set; }
        public object Camera { get; set; }
        public string ImgSrc { get; set; }
        public string EarthDate { get; set; }
        public object Rover { get; set; }
    }
}
