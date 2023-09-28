using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels
{
    public class Rover
    {
        [BsonElement("_id")]
        public int RoverId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("LandingDate")]
        public DateTime LandingDate { get; set; }

        [BsonElement("LaunchDate")]
        public DateTime LaunchDate { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("MaxSol")]
        public int MaxSol { get; set; }

        [BsonElement("MaxDate")]
        public DateTime MaxDate { get; set; }

        [BsonElement("TotalPhotos")]
        public int TotalPhotos { get; set; }

        [BsonElement("Cameras")]
        public List<RoverCamera> Cameras { get; set; }
    }
}
