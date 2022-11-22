using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class Boarding
{
    public Boarding()
    {
        _id = ObjectId.GenerateNewId();
    }

    [BsonId]
    public ObjectId _id { get; set; }
    
    [BsonElement("AnimalName")]
    public string AnimalName { get; set; }
    
    [BsonElement("AnimalAge")]
    public int AnimalAge { get; set; }
    
    [BsonElement("AnimalType")]
    public string AnimalType { get; set; }
    
    [BsonElement("PricePerNight")]
    public int PricePerNight { get; set; }
    
    [BsonElement("Notes")]
    public string Notes { get; set; }
    
    [BsonElement("DropOffTime")]
    public int DropOffTime { get; set; }
    
    [BsonElement("PickUpTime")]
    public int PickUpTime { get; set; }
    
    [BsonElement("StartDate")]
    public DateTime StartDate { get; set; }
    
    [BsonElement("EndDate")]
    public DateTime EndDate { get; set; }

    [BsonElement("IsCheckedIn")]
    public bool IsCheckedIn { get; set; }
}