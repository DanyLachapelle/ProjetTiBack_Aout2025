namespace Application.User_account.commands.resetPassword;

public class UserAccountResetPasswordOutput
{
    public string Message { get; }

    public UserAccountResetPasswordOutput(string message)
    {
        Message = message;
    }
}