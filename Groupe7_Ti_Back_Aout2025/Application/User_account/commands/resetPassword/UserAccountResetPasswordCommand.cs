namespace Application.User.commands.resetPassword;

public class UserAccountResetPasswordCommand
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
}