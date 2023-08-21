using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.Security.POCOs
{
    public enum ApiKeyStatus
    {
        Active,
        Revoked
    }

    public class ApiKey
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public long KeyId { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string KeyValue { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ApiKeyStatus Status { get; set; }
        public long UserId { get; set; }
        public int UsageCount { get; set; }
        public int RequestsThisHour { get; set; }
    }
}
