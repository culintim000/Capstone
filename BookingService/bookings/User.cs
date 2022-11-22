using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class User
{
    [BsonId]
    public ObjectId _id { get; set; }
    
    [BsonElement("Email")]
    public string Email { get; set; }
    
    [BsonElement("DayCareBookings")]
    public List<Daycare> DayCareBookings { get; set; }
    
    [BsonElement("BoardingBookings")]
    public List<Boarding> BoardingBookings { get; set; }
}