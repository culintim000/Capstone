using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Item {
    [BsonId]
    public ObjectId _id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    [BsonElement("Description")]
    public string Description { get; set; }
    [BsonElement("Price")]
    public double Price { get; set; }

    public Item()
    {
        _id = ObjectId.GenerateNewId();
    }
}