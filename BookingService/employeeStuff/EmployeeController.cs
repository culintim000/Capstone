using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingService;

[ApiController]
[Route("emp")]
public class EmployeeController : ControllerBase
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<User> _colUsers;
    private readonly IMongoCollection<CheckedInDayCare> _colCheckedInDayCares;
    private readonly IMongoCollection<CheckedInBoarding> _colCheckedInBoardings;
    private readonly IMongoCollection<EmployeeTasks> _colEmployeeTasks;

    public EmployeeController(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("Database1"));
        _db = client.GetDatabase("Capstone");
        _colUsers = _db.GetCollection<User>("UserBookings");
        _colCheckedInDayCares = _db.GetCollection<CheckedInDayCare>("CheckedInDayCares");
        _colCheckedInBoardings = _db.GetCollection<CheckedInBoarding>("CheckedInBoardings");
        _colEmployeeTasks = _db.GetCollection<EmployeeTasks>("EmployeeTasks");
    }

    [HttpGet]
    [Route("tasks")]
    public async Task<ActionResult<List<TaskToDo>>> GetDayCareTasks()
    {
        var fullTime = DateTime.Now;
        var checkedInDayCares = _colCheckedInDayCares.Find(_ => true).ToList();

        var dayCareTasks = new List<TaskToDo>();
        foreach (var checkedInDayCare in checkedInDayCares)
        {
            // Console.WriteLine(checkedInDayCare.Daycare.AnimalName);
            foreach (var task in checkedInDayCare.DayCareTasks)
            {
                // Console.WriteLine(task.Name);
                if (task.IsCompleted == false && task.IsPickedUp == false && task.StartHour < fullTime.Hour + 2)
                {
                    dayCareTasks.Add(task);
                }
            }
        }

        var checkedInBoardings = _colCheckedInBoardings.Find(_ => true).ToList();
        var boardingTasks = new List<TaskToDo>();
        foreach (var checkedInBoarding in checkedInBoardings)
        {
            var lastCompletedTask = checkedInBoarding.BoardingTasks.FindLastIndex(task => task.IsCompleted);
            // ret += lastCompletedTask;

            var indexToStart = lastCompletedTask + 1;

            for (int i = indexToStart; i < indexToStart + 5; i++)
            {
                if (checkedInBoarding.BoardingTasks[i].IsCompleted == false &&
                    checkedInBoarding.BoardingTasks[i].IsPickedUp == false &&
                    checkedInBoarding.BoardingTasks[i].StartHour < fullTime.Hour + 2)
                {
                    boardingTasks.Add(checkedInBoarding.BoardingTasks[i]);
                }
            }
        }

        var oneList = new List<TaskToDo>();
        oneList.AddRange(boardingTasks);
        oneList.AddRange(dayCareTasks);
        oneList = oneList.OrderBy(x => x.StartHour).ThenBy(x => x.StartMinute).ToList();
        return Ok(oneList);
    }

    [HttpPost("pickUpTask")]
    public async Task<ActionResult<EmployeeTasks>> PickUpTask(string email, [FromBody] TaskToDo task)
    {
        if (task == null)
        {
            return NotFound("Task not found");
        }

        var employee = _colEmployeeTasks.Find(x => x.Email == email).FirstOrDefault();

        if (employee == null)
        {
            task.IsPickedUp = true;
            employee = new EmployeeTasks(email, task);
            await _colEmployeeTasks.InsertOneAsync(employee);
        }
        else
        {
            task.IsPickedUp = true;
            employee.Tasks.Add(task);
            await _colEmployeeTasks.ReplaceOneAsync(x => x.Email == email, employee);
        }

        FindUpdateTask(task);
        return Ok(employee);
    }

    [HttpPost("dropTask")]
    public async Task<ActionResult<EmployeeTasks>> DropTask(string email, [FromBody] TaskToDo task)
    {
        if (task == null)
        {
            return NotFound("Task to drop not found");
        }

        var employee = _colEmployeeTasks.Find(x => x.Email == email).FirstOrDefault();

        if (employee == null)
        {
            return NotFound("No employee found");
        }
        
        employee.Tasks.RemoveAt(employee.Tasks.FindIndex(x => x.Name == task.Name && x._AppointmentId == task._AppointmentId));
        await _colEmployeeTasks.ReplaceOneAsync(x => x.Email == email, employee);
        task.IsPickedUp = false;

        FindUpdateTask(task);
        return Ok(employee);
    }
    
    [HttpPost("rescheduleTask")]
    public async Task<ActionResult<TaskToDo>> RescheduleTask(string email, int hour, int minute, [FromBody] TaskToDo task)
    {
        if (task == null)
        {
            return NotFound("Task to drop not found");
        }

        var employee = _colEmployeeTasks.Find(x => x.Email == email ).FirstOrDefault();
        if (employee != null && (employee.Tasks.Count != 0 || employee.Tasks != null))
        {
            TaskToDo taskFound = employee.Tasks.Find(x => x.Name == task.Name && x._AppointmentId == task._AppointmentId);

            if (taskFound != null)
            {
                employee.Tasks.Remove(taskFound);
                await _colEmployeeTasks.ReplaceOneAsync(x => x.Email == email, employee);
            }
            await _colEmployeeTasks.ReplaceOneAsync(x => x.Email == email, employee);
        }

        // Console.WriteLine(task.ToString());
        task.StartHour += hour;
        task.StartMinute += minute;
        
        if (task.StartMinute >= 60)
        {
            task.StartHour += 1;
            task.StartMinute -= 60;
        }
        
        task.IsPickedUp = false;
        // Console.WriteLine(task.ToString());
        FindUpdateTask(task);
        return Ok(task);
    }
    
    [HttpPost("completeTask")]
    public async Task<ActionResult<TaskToDo>> CompleteTask(string email, [FromBody] TaskToDo task)
    {
        if (task == null)
        {
            return NotFound("Task to drop not found");
        }

        var employee = _colEmployeeTasks.Find(x => x.Email == email).FirstOrDefault();

        if (employee == null)
        {
            return NotFound("No employee found");
        }
        
        employee.Tasks.RemoveAt(employee.Tasks.FindIndex(x => x.Name == task.Name && x._AppointmentId == task._AppointmentId));
        await _colEmployeeTasks.ReplaceOneAsync(x => x.Email == email, employee);
        task.IsPickedUp = false;
        task.IsCompleted = true;

        FindUpdateTask(task);
        return Ok(employee);
    }

    private void FindUpdateTask(TaskToDo task)
    {
        //get checked in day care
        var checkedInDayCare = _colCheckedInDayCares
            .Find(x => x.DayCareTasks.Any(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId))
            .FirstOrDefault();

        if (checkedInDayCare != null)
        {
            //get the task in daycare
            var oldTask =
                checkedInDayCare.DayCareTasks.Find(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId);
            oldTask.IsPickedUp = task.IsPickedUp;
            oldTask.IsCompleted = task.IsCompleted;
            oldTask.StartHour = task.StartHour;
            oldTask.StartMinute = task.StartMinute;

            //Update task in daycares
            _colCheckedInDayCares.ReplaceOne(
                x => x.DayCareTasks.Any(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId),
                checkedInDayCare);
        }
        else
        {
            var checkedInBoarding = _colCheckedInBoardings
                .Find(x => x.BoardingTasks.Any(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId))
                .FirstOrDefault();
            if (checkedInBoarding != null)
            {
                //get the task in boarding
                var oldTask =
                    checkedInBoarding.BoardingTasks.Find(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId);
                oldTask.IsPickedUp = task.IsPickedUp;
                oldTask.IsCompleted = task.IsCompleted;
                oldTask.StartHour = task.StartHour;
                oldTask.StartMinute = task.StartMinute;
                
                if (task.StartMinute >= 60)
                {
                    task.StartHour += 1;
                    task.StartMinute -= 60;
                }

                //Update task in daycares
                _colCheckedInBoardings.ReplaceOne(
                    x => x.BoardingTasks.Any(t => t.Name == task.Name && t._AppointmentId == task._AppointmentId),
                    checkedInBoarding);
            }
        }
    }
    
    [HttpGet("getEmployeeTasks")]
    public ActionResult<List<TaskToDo>> GetEmployeeTasks(string email)
    {
        var employee = _colEmployeeTasks.Find(x => x.Email == email).FirstOrDefault();
        if (employee == null)
        {
            return NotFound("No employee found");
        }

        return Ok(employee.Tasks);
    }
}