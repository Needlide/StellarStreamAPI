using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs
{
    public class ApiKeyConsumer
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
