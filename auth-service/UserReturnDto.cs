public class UserReturnDto 
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public bool isAdmin { get; set; } = false;
    public bool isWorker { get; set; } = false;

    public UserReturnDto(string Name, string Email, string Token, bool isAdmin, bool isWorker )
    {
        this.Name = Name;
        this.Email = Email;
        this.Token = Token;
        this.isAdmin = isAdmin;
        this.isWorker = isWorker;
    }
}