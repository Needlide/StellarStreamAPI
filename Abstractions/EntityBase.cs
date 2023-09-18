using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.Abstraction
{
    public abstract class EntityBase
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }
}
