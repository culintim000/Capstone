using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingService;

[ApiController]
[Route("book")]
public class BookingController : ControllerBase
{
    private DateTime today = DateTime.Now.Date.Add(new TimeSpan(0, 0, 0));
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<User> _col;
    private readonly IMongoCollection<CheckedInDayCare> _colCheckedInDayCares;
    private readonly IMongoCollection<CheckedInBoarding> _colCheckedInBoardings;
    private readonly IMongoCollection<EmployeeTasks> _colEmployeeTasks;

    public BookingController(IConfiguration config) {
        var client = new MongoClient(config.GetConnectionString("Database1"));
        _db = client.GetDatabase("Capstone");
        _col = _db.GetCollection<User>("UserBookings");
        _colCheckedInDayCares = _db.GetCollection<CheckedInDayCare>("CheckedInDayCares");
        _colCheckedInBoardings = _db.GetCollection<CheckedInBoarding>("CheckedInBoardings");
        _colEmployeeTasks = _db.GetCollection<EmployeeTasks>("EmployeeTasks");
    }

    [HttpPost]
    public async Task<ActionResult<string>> BookDayCare(InputObject input)
    {
        User user = _col.Find(u => u.Email == input.Email).FirstOrDefault();
        
        if (input.BookingType == "Daycare")
        {
            Daycare daycare = new Daycare();
            daycare.AnimalName = input.AnimalName;
            daycare.AnimalAge = input.AnimalAge;
            daycare.AnimalType = input.AnimalType;
            daycare.PricePerHour = input.Price;
            daycare.Notes = input.Notes;
            daycare.DropOffTime = input.DropOffTime;
            daycare.PickUpTime = input.PickUpTime;
            daycare.StartDate = input.StartDate.Date.Add(new TimeSpan(0, 0, 0));
            daycare.EndDate = input.EndDate.Date.Add(new TimeSpan(0, 0, 0));
            daycare.IsCheckedIn = false;
            
            if (user == null)
            {
                await _col.InsertOneAsync(new User
                {
                    Email = input.Email,
                    DayCareBookings = new List<Daycare>() {daycare}
                });
            }
            else
            {
                if (user.DayCareBookings == null)
                {
                    user.DayCareBookings = new List<Daycare>() {daycare};
                }
                else
                {
                    user.DayCareBookings.Add(daycare);
                }
                await _col.ReplaceOneAsync(u => u.Email == input.Email, user);
            }

            return Ok(daycare._id.ToString());
        }
        else
        {
            Boarding boarding = new Boarding();
            boarding.AnimalName = input.AnimalName;
            boarding.AnimalAge = input.AnimalAge;
            boarding.AnimalType = input.AnimalType;
            boarding.PricePerNight = input.Price;
            boarding.Notes = input.Notes;
            boarding.DropOffTime = input.DropOffTime;
            boarding.PickUpTime = input.PickUpTime;
            boarding.StartDate = input.StartDate.Date.Add(new TimeSpan(0, 0, 0));
            boarding.EndDate = input.EndDate.Date.Add(new TimeSpan(0, 0, 0));
            boarding.IsCheckedIn = false;
            
            if (user == null)
            {
                await _col.InsertOneAsync(new User
                {
                    Email = input.Email,
                    BoardingBookings = new List<Boarding>() {boarding}
                });
            }
            else
            {
                if (user.BoardingBookings == null)
                {
                    user.BoardingBookings = new List<Boarding>() {boarding};
                }
                else
                {
                    user.BoardingBookings.Add(boarding);
                }
                await _col.ReplaceOneAsync(u => u.Email == input.Email, user);
            }
            return Ok(boarding._id.ToString());
        }
    }

    [HttpGet("daycares")]
    public async Task<ActionResult<List<DaycareForDisplaying>>> GetDayCaresForDisplay(string email)
    {
        var result = GetDayCares(email);
        if(result.Result == null)
        {
            return NotFound();
        }
        
        List<Daycare> allDaycares = result.Result;
        List<DaycareForDisplaying> daycareForDisplayings = new List<DaycareForDisplaying>();
        foreach (var daycare in allDaycares)
        {
            daycareForDisplayings.Add(new DaycareForDisplaying(daycare));
        }
        
        return Ok(daycareForDisplayings);
    }

    [HttpPost("thisShouldntBeCalledByAnyoneButTHisIsMatchingTwoEnDPoiNTs")]
    public async Task<List<Daycare>> GetDayCares(string email)
    {
        User user = _col.Find(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            return null;
        }
        if (user.DayCareBookings == null)
        {
            return null;
        }
        
        var tempList = new List<Daycare>();
        foreach (var daycare in user.DayCareBookings)
        {
            var result = DateTime.Compare(daycare.EndDate , today);
            if (result < 0)
            {
                tempList.Add(daycare);
            }
        }

        foreach (var daycare in tempList)
        {
            user.DayCareBookings.Remove(daycare);
        }
        
        var sorted = (from p in user.DayCareBookings
            orderby p.StartDate
            select p).ToList();
        
        await _col.ReplaceOneAsync(u => u.Email == email, user);
        return sorted;
    }

    [HttpGet("boardings")]
    public async Task<ActionResult<List<BoardingForDisplaying>>> GetBoardingsForUser(string email)
    {
        var result = GetBoardings(email);
        if(result.Result == null)
        {
            return NotFound();
        }
        
        List<Boarding> allBoardings = result.Result;
        List<BoardingForDisplaying> boardingsForDisplaying = new List<BoardingForDisplaying>();
        foreach (var boarding in allBoardings)
        {
            boardingsForDisplaying.Add(new BoardingForDisplaying(boarding));
        }
        
        return boardingsForDisplaying;
    }
    
    [HttpPost("thisShouldntBeCalledByAnyoneButTHisIsMatchingTwoEnDPoiNTsSoThisIsTheBoardingOne")]
    public async Task<List<Boarding>> GetBoardings(string email)
    {
        User user = _col.Find(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            return null;
        }
        if (user.BoardingBookings == null)
        {
            return null;
        }

        var tempList = new List<Boarding>();
        foreach (var boarding in user.BoardingBookings)
        {
            var result = DateTime.Compare(boarding.EndDate , today);
            if (result < 0)
            {
                tempList.Add(boarding);
            }
        }

        foreach (var boarding in tempList)
        {
            user.BoardingBookings.Remove(boarding);
        }
        
        var sorted = (from p in user.BoardingBookings
            orderby p.StartDate
            select p).ToList();

        await _col.ReplaceOneAsync(u => u.Email == email, user);
        return sorted;
    }

    [HttpGet("todaysDaycares")]
    public async Task<ActionResult<List<DaycareForEmployees>>> GetTodaysBookings()
    {
        var tempList = new List<DaycareForEmployees>();
        List<User> users = _col.Find(u => u.DayCareBookings != null).ToList();
        
        foreach (var user in users)
        {
            var removeDaycares = new List<Daycare>();
            foreach (var daycare in user.DayCareBookings)
            {
                var lastDayPassed = DateTime.Compare(daycare.EndDate , today);
                if (lastDayPassed < 0)
                {
                    removeDaycares.Add(daycare);
                }
                else
                {
                    var result = DateTime.Compare(daycare.StartDate , today);
                    if (result <= 0)
                    {
                        if (daycare.IsCheckedIn == false)
                        {
                            var daycareForEmployees = new DaycareForEmployees(daycare, user.Email);
                            // tempList.Add(daycare);
                            tempList.Add(daycareForEmployees);
                        }
                    }
                }
            }

            foreach (var removeDaycare in removeDaycares)
            {
                user.DayCareBookings.Remove(removeDaycare);
            }
            await _col.ReplaceOneAsync(u => u.Email == user.Email, user);
        }
        
        var sortedByTime = (from p in tempList
            orderby p.DropOffTime
            select p).ToList();
        if (sortedByTime.Count == 0)
        {
            return NotFound();
        }
        return sortedByTime;
    }

    [HttpGet("todaysBoardings")]
    public async Task<ActionResult<List<BoardingForEmployees>>> GetTodaysBoardings()
    {
        var tempList = new List<BoardingForEmployees>();
        List<User> users = _col.Find(u => u.BoardingBookings != null).ToList();
        
        foreach (var user in users)
        {
            var removeBoardings = new List<Boarding>();
            foreach (var boarding in user.BoardingBookings)
            {
                var lastDayPassed = DateTime.Compare(boarding.EndDate , today);
                if (lastDayPassed < 0)
                {
                    removeBoardings.Add(boarding);
                }
                else
                {
                    var result = DateTime.Compare(boarding.StartDate , today);
                    if (result <= 0)
                    {
                        if (boarding.IsCheckedIn == false)
                        {
                            var boardingForEmployees = new BoardingForEmployees(boarding, user.Email);
                            tempList.Add(boardingForEmployees);
                            // tempList.Add(boarding);
                        }
                    }
                }
            }

            foreach (var removeBoarding in removeBoardings)
            {
                user.BoardingBookings.Remove(removeBoarding);
            }
            await _col.ReplaceOneAsync(u => u.Email == user.Email, user);
        }
        
        var sortedByTime = (from p in tempList
            orderby p.DropOffTime
            select p).ToList();
        if (sortedByTime.Count == 0)
        {
            return NotFound();
        }
        return sortedByTime;
    }
    
    [HttpPost("checkInDaycare")]
    public async Task<ActionResult> CheckInDaycare(string email, string animalName)
    {
        User user = _col.Find(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        if (user.DayCareBookings == null)
        {
            return NotFound();
        }
        
        var atLeastOneCheckedIn = false;
        foreach (var daycare in user.DayCareBookings)
        {
            if (daycare.AnimalName == animalName)
            {
                daycare.IsCheckedIn = true;
                var checkin = new CheckedInDayCare(daycare, user.Email);
                _colCheckedInDayCares.InsertOne(checkin);
                atLeastOneCheckedIn = true;
            }
        }
        
        if (atLeastOneCheckedIn == false)
        {
            return NotFound();
        }
        
        await _col.ReplaceOneAsync(u => u.Email == email, user);
        return Ok();
    }
    
    [HttpPost("checkInBoarding")]
    public async Task<ActionResult> CheckInBoarding(string email, string animalName)
    {
        User user = _col.Find(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        if (user.BoardingBookings == null)
        {
            return NotFound();
        }
        
        var atLeastOneCheckedIn = false;
        foreach (var boarding in user.BoardingBookings)
        {
            if (boarding.AnimalName == animalName)
            {
                boarding.IsCheckedIn = true;
                var checkin = new CheckedInBoarding(boarding, user.Email);
                _colCheckedInBoardings.InsertOne(checkin);
                atLeastOneCheckedIn = true;
            }
        }
        
        if (atLeastOneCheckedIn == false)
        {
            return NotFound();
        }
        await _col.ReplaceOneAsync(u => u.Email == email, user);
        return Ok();
    }
    
    [HttpGet("searchForBoardings")]
    public async Task<ActionResult<List<BoardingForEmployees>>> BoardingsForUser(string email)
    {
        var result = GetBoardings(email);
        if(result.Result == null)
        {
            return NotFound();
        }
        
        List<Boarding> allBoardings = result.Result;

        var returnList = new List<BoardingForEmployees>();
        foreach (var boarding in allBoardings)
        {
            BoardingForEmployees boardingForEmployees = new BoardingForEmployees(boarding, email);
            returnList.Add(boardingForEmployees);
        }

        return returnList;
    }
    
    [HttpGet("searchForDayCares")]
    public async Task<ActionResult<List<DaycareForEmployees>>> DayCaresForUser(string email)
    {
        var result = GetDayCares(email);
        if(result.Result == null)
        {
            return NotFound();
        }
        
        List<Daycare> allDayCares = result.Result;

        var returnList = new List<DaycareForEmployees>();
        foreach (var daycare in allDayCares)
        {
            DaycareForEmployees daycareForEmployees = new DaycareForEmployees(daycare, email);
            returnList.Add(daycareForEmployees);
        }

        return returnList;
    }
    
    [HttpGet("getCheckedInDayCares")]
    public async Task<ActionResult<List<CheckedInDayCare>>> GetCheckedInDayCares()
    {
        List<CheckedInDayCare> checkedInDayCares = _colCheckedInDayCares.Find(c => true).ToList();
        if (checkedInDayCares == null)
        {
            return NotFound();
        }
        return checkedInDayCares;
    }
    
    [HttpGet("getCheckedInBoardings")]
    public async Task<ActionResult<List<CheckedInBoarding>>> GetCheckedInBoardings()
    {
        List<CheckedInBoarding> checkedInBoardings = _colCheckedInBoardings.Find(c => true).ToList();
        if (checkedInBoardings == null)
        {
            return NotFound();
        }
        return checkedInBoardings;
    }
    
    [HttpPost("checkOutDaycare")]
    public async Task<ActionResult> CheckOutDaycare(string email, string animalName)
    {
        CheckedInDayCare checkedInDayCare = _colCheckedInDayCares.Find(c => c.Email == email && c.Daycare.AnimalName == animalName).FirstOrDefault();
        if (checkedInDayCare == null)
        {
            return NotFound();
        }
        
        var user = _col.Find(u => u.Email == email).FirstOrDefault();
        
        if (user == null)
        {
            return NotFound();
        }

        foreach (var dayCareBooking in user.DayCareBookings)
        {
            if (dayCareBooking.AnimalName == animalName)
            {
                dayCareBooking.IsCheckedIn = false;
            }
        }

        var employees = _colEmployeeTasks.Find(_ => true).ToList();
        foreach (var employee in employees)
        {
            var tasksToRemove = new List<TaskToDo>();
            foreach (var task in employee.Tasks)
            {
                if (task._AppointmentId == checkedInDayCare.Daycare._id.ToString())
                {
                    tasksToRemove.Add(task);
                }
            }
            foreach (var taskToDo in tasksToRemove)
            {
                employee.Tasks.Remove(taskToDo);
            }
            await _colEmployeeTasks.ReplaceOneAsync(e => e.Email == employee.Email, employee);
        }
        
        await _col.ReplaceOneAsync(u => u.Email == user.Email, user);
        await _colCheckedInDayCares.DeleteOneAsync(c => c.Email == email && c.Daycare.AnimalName == animalName);
        return Ok("Checked out");
    }
    
    [HttpPost("checkOutBoarding")]
    public async Task<ActionResult> CheckOutBoarding(string email, string animalName)
    {
        CheckedInBoarding checkedInBoarding = _colCheckedInBoardings.Find(c => c.Email == email && c.Boarding.AnimalName == animalName).FirstOrDefault();
        if (checkedInBoarding == null)
        {
            return NotFound();
        }
        
        var user = _col.Find(u => u.Email == email).FirstOrDefault();
        
        if (user == null)
        {
            return NotFound();
        }

        foreach (var boardingBooking in user.BoardingBookings)
        {
            if (boardingBooking.AnimalName == animalName)
            {
                boardingBooking.IsCheckedIn = false;
            }
        }
        
        var employees = _colEmployeeTasks.Find(_ => true).ToList();
        foreach (var employee in employees)
        {
            var tasksToRemove = new List<TaskToDo>();
            foreach (var task in employee.Tasks)
            {
                if (task._AppointmentId == checkedInBoarding.Boarding._id.ToString())
                {
                    tasksToRemove.Add(task);
                }
            }

            foreach (var taskToDo in tasksToRemove)
            {
                employee.Tasks.Remove(taskToDo);
            }
            await _colEmployeeTasks.ReplaceOneAsync(e => e.Email == employee.Email, employee);
        }
        
        await _col.ReplaceOneAsync(u => u.Email == user.Email, user);
        await _colCheckedInBoardings.DeleteOneAsync(c => c.Email == email && c.Boarding.AnimalName == animalName);
        return Ok("Checked out");
    }
    
    [HttpGet("getCheckedInDaycareWithId")]
    public async Task<ActionResult<Daycare>> GetDaycare(string id)
    {
        ObjectId objectId = new ObjectId(id);

        foreach (var checkedInDayCare in _colCheckedInDayCares.Find(c => true).ToList())
        {
            if (checkedInDayCare.Daycare._id == objectId)
            {
                return checkedInDayCare.Daycare;
            }
        }
        // Console.WriteLine(objectId);
        return NotFound();
    }

    [HttpGet("getCheckedInBoardingWithId")]
    public async Task<ActionResult<Boarding>> GetBoarding(string id)
    {
        ObjectId objectId = new ObjectId(id);
        
        foreach (var checkedInBoarding in _colCheckedInBoardings.Find(c => true).ToList())
        {
            if (checkedInBoarding.Boarding._id == objectId)
            {
                return  Ok(checkedInBoarding.Boarding);
            }
        }

        return NotFound();
    }
}