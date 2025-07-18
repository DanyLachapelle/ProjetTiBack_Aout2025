namespace Application.User.commands.changePassword;

public class UserChangePasswordCommand
{
    public UserChangePasswordCommand(string oldPassword, string newPassword)
    {
        this.oldPassword = oldPassword;
        this.newPassword = newPassword;
    }
    
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
    
    public string pseudo { get; set; } = string.Empty;
}