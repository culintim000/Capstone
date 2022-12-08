using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class UserCart {
    [BsonId]
    public ObjectId _id { get; set; }
    [BsonElement("Lines")]
    public List<Listing> Lines { get; set; } = new();
    public UserCart(ObjectId Id) {
        this._id = _id;
    }
}