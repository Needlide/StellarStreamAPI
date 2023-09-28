using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels
{
    public class RoverCamera
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; }
    }
}
