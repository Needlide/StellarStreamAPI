using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Security
{
    public enum ApiKeyStatus
    {
        Active,
        Revoked
    }

    public class ApiKey
    {
        [BsonId]
        public ObjectId KeyId { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string KeyValue { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMonths(6);
        public ApiKeyStatus Status { get; set; } = ApiKeyStatus.Active;
        public long UserId { get; set; }
        public int UsageCount { get; set; } = 0;
        public int RequestsThisHour { get; set; } = 0;
    }
}
