using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.Security.POCOs
{
    public class ApiKeyConsumer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public long KeyId { get; set; }
    }
}
