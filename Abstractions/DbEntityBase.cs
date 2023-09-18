using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StellarStreamAPI.Abstraction
{
    public abstract class DbEntityBase
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }
}
