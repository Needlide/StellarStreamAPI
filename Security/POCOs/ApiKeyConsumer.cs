using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.Security.POCOs
{
    public class ApiKeyConsumer
    {
        [BsonId]
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public long KeyId { get; set; }
    }
}
