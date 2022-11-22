namespace BookingService;

public class BoardingForEmployees
{
    public BoardingForEmployees(Boarding boarding, string email)
    {
        AnimalName = boarding.AnimalName;
        AnimalType = boarding.AnimalType;
        AnimalAge = boarding.AnimalAge;
        PricePerNight = boarding.PricePerNight;
        StartDate = boarding.StartDate;
        EndDate = boarding.EndDate;
        Notes = boarding.Notes;
        DropOffTime = boarding.DropOffTime;
        PickUpTime = boarding.PickUpTime;
        IsCheckedIn = boarding.IsCheckedIn;
        OwnerEmail = email;
        Id = boarding._id.ToString();
    }
    
    
    public string Id { get; set; }
    public string AnimalName { get; set; }
    
    public int AnimalAge { get; set; }
    
    public string AnimalType { get; set; }
    
    public int PricePerNight { get; set; }
    
    public string Notes { get; set; }
    
    public int DropOffTime { get; set; }
    
    public int PickUpTime { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public bool IsCheckedIn { get; set; }
    
    public string OwnerEmail { get; set; }
}