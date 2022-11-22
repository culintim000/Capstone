
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class TaskToDo
{
    public TaskToDo(string appointmentId, string name, string description, int startHour, int startMinute)
    {
        _AppointmentId = appointmentId;
        Name = name;
        Description = description;
        StartHour = startHour;
        StartMinute = startMinute;
        IsCompleted = false;
        IsPickedUp = false;
    }
    
    public TaskToDo(string appointmentId, string name, string description, int startHour, int startMinute, bool isCompleted, bool isPickedUp)
    {
        _AppointmentId = appointmentId;
        Name = name;
        Description = description;
        StartHour = startHour;
        StartMinute = startMinute;
        IsCompleted = isCompleted;
        IsPickedUp = isPickedUp;
    }
    
    public TaskToDo()
    {
    }

    [BsonElement("_AppointmentId")]
    public string _AppointmentId { get; set; }
    
    [BsonElement("Name")]
    public string Name { get; set; }
    
    [BsonElement("Description")]
    public string Description { get; set; }
    
    [BsonElement("StartHour")]
    public int StartHour { get; set; }
    
    [BsonElement("StartMinute")]
    public int StartMinute { get; set; }
    
    [BsonElement("IsCompleted")]
    public bool IsCompleted { get; set; }
    [BsonElement("IsPickedUp")]
    public bool IsPickedUp { get; set; }
    
    public override string ToString()
    {
        return $"Task: {Name} - {Description} - {StartHour}:{StartMinute} - {IsCompleted} - {IsPickedUp}";
    }
}