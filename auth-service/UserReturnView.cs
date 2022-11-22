namespace auth_service;

public class UserReturnView
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool isWorker { get; set; } = false;

    public UserReturnView(string Name, string Email, string Phone, bool isWorker )
    {
        this.Name = Name;
        this.Email = Email;
        this.Phone = Phone;
        this.isWorker = isWorker;
    }
}