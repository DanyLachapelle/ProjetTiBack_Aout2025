namespace Application.User.commands.forgotPassword;

public class UserAccountForgotPasswordOutput
{
    public string Message { get; }

    public UserAccountForgotPasswordOutput(string message)
    {
        Message = message;
    }
}