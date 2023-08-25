using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.POCOs.Security
{
    public class ApiKey
    {
        [BsonId]
        public ObjectId KeyId { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string KeyValue { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddMonths(6);
        public long UserId { get; set; }
        public int RequestsThisHour { get; set; } = 0;
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;
        public DateTime LastUsed { get; set; }
        public bool NeedsToReset => LastUsed.AddHours(1) < DateTime.UtcNow;
    }
}
