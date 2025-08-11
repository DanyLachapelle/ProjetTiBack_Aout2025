using Application.User_account.commands.resetPassword;
using Application.User.commands.changePassword;
using Application.User.commands.forgotPassword;
using Application.User.commands.login;
using Application.User.commands.resetPassword;
using Application.Utils;

namespace Application.User.commands;

public class UserAccountCommandProcessor
{
    private readonly ICommandHandler<UserAccountLoginCommand,UserAccountLoginOutput> _userLoginHandler;
    private readonly ICommandHandler<UserAccountChangePasswordCommand,UserAccountChangePasswordOutput> _userChangePasswordHandler;
    private readonly ICommandHandler<UserAccountForgotPasswordCommand, UserAccountForgotPasswordOutput> _forgotPasswordHandler;
    private readonly ICommandHandler<UserAccountResetPasswordCommand, UserAccountResetPasswordOutput> _resetPasswordHandler;

    
    public UserAccountCommandProcessor(ICommandHandler<UserAccountLoginCommand, UserAccountLoginOutput> userLoginHandler,
        ICommandHandler<UserAccountChangePasswordCommand, UserAccountChangePasswordOutput> userChangePasswordHandler, ICommandHandler<UserAccountForgotPasswordCommand, UserAccountForgotPasswordOutput> forgotPasswordHandler, ICommandHandler<UserAccountResetPasswordCommand, UserAccountResetPasswordOutput> resetPasswordHandler)
    {
        _userLoginHandler = userLoginHandler;
        _userChangePasswordHandler = userChangePasswordHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _resetPasswordHandler = resetPasswordHandler;
    }
    
    public UserAccountLoginOutput Login(UserAccountLoginCommand command)
    {
        return _userLoginHandler.Handle(command);
    }
    
    public UserAccountChangePasswordOutput ChangePassword(UserAccountChangePasswordCommand command)
    {
        return _userChangePasswordHandler.Handle(command);
    }

    public UserAccountForgotPasswordOutput ForgotPassword(UserAccountForgotPasswordCommand command)
    {
        return _forgotPasswordHandler.Handle(command);
    }
    
    public UserAccountResetPasswordOutput ResetPassword(UserAccountResetPasswordCommand command)
    {
        return _resetPasswordHandler.Handle(command);
    }
}