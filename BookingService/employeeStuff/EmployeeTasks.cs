using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class EmployeeTasks
{
    public EmployeeTasks(string email, List<TaskToDo> tasks)
    {
        _id = ObjectId.GenerateNewId();
        Email = email;
        Tasks = tasks;
    }

    public EmployeeTasks(string email, TaskToDo task)
    {
        _id = ObjectId.GenerateNewId();
        Email = email;
        Tasks = new List<TaskToDo> { task };
    }

    public EmployeeTasks(ObjectId _id, string email, TaskToDo task)
    {
        this._id = _id;
        Email = email;
        if (Tasks == null)
        {
            Tasks = new List<TaskToDo>();
        }
        Tasks.Add(task);
    }

    [BsonId]
    private ObjectId _id { get; set; }
    [BsonElement("Email")]
    public string Email { get; set; }
    [BsonElement("Tasks")]
    public List<TaskToDo> Tasks { get; set; }
}