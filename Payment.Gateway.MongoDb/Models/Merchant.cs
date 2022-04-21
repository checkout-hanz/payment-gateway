using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.Gateway.MongoDb.Models
{
    public class Merchant
    {
        [BsonId]
        public Guid MerchantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
