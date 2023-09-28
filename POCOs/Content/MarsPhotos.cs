using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels;

namespace StellarStreamAPI.POCOs.Content
{
    public class MarsPhotos
    {
        [BsonElement("Sol")]
        public int Sol { get; set; }

        [BsonElement("Camera")]
        public Camera Camera { get; set; }

        [BsonElement("ImgSrc")]
        public string ImgSrc { get; set; }

        [BsonElement("EarthDate")]
        public DateTime EarthDate { get; set; }

        [BsonElement("Rover")]
        public Rover Rover { get; set; }
    }
}
