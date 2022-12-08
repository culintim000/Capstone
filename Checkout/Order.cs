using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Order {
    [BsonId]
    public ObjectId _id { get; set; }
    [BsonElement("Cart")]
    public UserCart Cart { get; set; }
    public Order() {
        _id = ObjectId.GenerateNewId();
    }
}