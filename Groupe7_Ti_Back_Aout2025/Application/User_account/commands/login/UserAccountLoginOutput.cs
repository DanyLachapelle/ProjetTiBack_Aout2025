namespace Application.User.commands.login;

public class UserAccountLoginOutput
{
    public string Token { get; set; }
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    
}