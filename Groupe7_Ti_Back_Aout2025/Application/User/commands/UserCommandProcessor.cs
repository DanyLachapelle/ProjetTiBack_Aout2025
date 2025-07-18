using Application.User.commands.changePassword;
using Application.User.commands.login;
using Application.Utils;

namespace Application.User.commands;

public class UserCommandProcessor
{
    private readonly IQueryHandler<UserLoginQuery,UserLoginOutput> _userLoginHandler;
    private readonly ICommandHandler<UserChangePasswordCommand,UserChangePasswordOutput> _userChangePasswordHandler;
    
    public UserCommandProcessor(IQueryHandler<UserLoginQuery, UserLoginOutput> userLoginHandler, ICommandHandler<UserChangePasswordCommand, UserChangePasswordOutput> userChangePasswordHandler)
    {
        _userLoginHandler = userLoginHandler;
        _userChangePasswordHandler = userChangePasswordHandler;
    }
    
    public UserLoginOutput Login(UserLoginQuery query)
    {
        return _userLoginHandler.Handle(query);
    }

    public UserChangePasswordOutput ChangePassword(UserChangePasswordCommand command)
    {
        return _userChangePasswordHandler.Handle(command);
    }
}