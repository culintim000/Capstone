using MongoDB.Bson;

namespace BookingService;

public class BoardingTask
{
    public BsonObjectId _BoardingId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeOnly StartTime { get; set; }
    public bool IsCompleted { get; set; }
}