namespace Application.User.commands.changePassword;

public class UserAccountChangePasswordCommand
{
    public UserAccountChangePasswordCommand(string oldPassword, string newPassword)
    {
        this.OldPassword = oldPassword;
        this.NewPassword = newPassword;
    }
    
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    
    public string Pseudo { get; set; } = string.Empty;
}