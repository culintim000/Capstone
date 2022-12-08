using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Listing {
    //[BsonElement("Item")]
    public Item? Item { get; set; }
    //[BsonElement("Quantity")]
    public int Quantity { get; set; } = 0;
    public Listing() {}
    public Listing(Item Item, int Quantity) {
        this.Item = Item;
        this.Quantity = Quantity;
    }
}