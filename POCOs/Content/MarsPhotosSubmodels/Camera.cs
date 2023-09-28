using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Content.MarsPhotosSubmodels
{
    public class Camera
    {
        [BsonElement("_id")]
        public int CameraId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("RoverId")]
        public int RoverId { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; }
    }
}
