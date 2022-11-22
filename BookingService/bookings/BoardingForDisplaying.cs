namespace BookingService;

public class BoardingForDisplaying
{ 
    public BoardingForDisplaying(Boarding boarding)
    {
        Id = boarding._id.ToString();
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
    }
    
    public string Id { get; set; }
    public string AnimalName { get; set; }
    public int AnimalAge { get; set; }
    public string AnimalType { get; set; }
    public string Notes { get; set; }

    public int DropOffTime { get; set; }
    public int PickUpTime { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int PricePerNight { get; set; }
    public bool IsCheckedIn { get; set; }

}