namespace BookingService;

public class DaycareForEmployees
{
    public DaycareForEmployees(Daycare daycare, string email)
    {
        AnimalName = daycare.AnimalName;
        AnimalType = daycare.AnimalType;
        AnimalAge = daycare.AnimalAge;
        PricePerHour = daycare.PricePerHour;
        StartDate = daycare.StartDate;
        EndDate = daycare.EndDate;
        Notes = daycare.Notes;
        DropOffTime = daycare.DropOffTime;
        PickUpTime = daycare.PickUpTime;
        IsCheckedIn = daycare.IsCheckedIn;
        OwnerEmail = email;
        Id = daycare._id.ToString();
    }

    
    public string Id { get; set; }
    public string AnimalName { get; set; }
    
    public int AnimalAge { get; set; }
    
    public string AnimalType { get; set; }
    
    public int PricePerHour { get; set; }
    
    public string Notes { get; set; }
    
    public int DropOffTime { get; set; }
    
    public int PickUpTime { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsCheckedIn { get; set; }
    
    public string OwnerEmail { get; set; }
}