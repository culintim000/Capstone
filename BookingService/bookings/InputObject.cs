namespace BookingService;

public class InputObject
{
    public string Email { get; set; }
    
    public string BookingType { get; set; }
    
    public string AnimalName { get; set; }
    
    public int AnimalAge { get; set; }
    
    public string AnimalType { get; set; }
    
    public int Price { get; set; }
    
    public string Notes { get; set; }
    
    public int DropOffTime { get; set; }
    
    public int PickUpTime { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}