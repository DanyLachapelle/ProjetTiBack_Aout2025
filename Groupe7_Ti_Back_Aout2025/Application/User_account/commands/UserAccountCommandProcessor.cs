using Application.User.commands.login;
using Application.Utils;

namespace Application.User.commands;

public class UserAccountCommandProcessor
{
    private readonly ICommandHandler<UserAccountLoginQuery,UserAccountLoginOutput> _userLoginHandler;
    
    public UserAccountCommandProcessor(ICommandHandler<UserAccountLoginQuery, UserAccountLoginOutput> userLoginHandler)
    {
        _userLoginHandler = userLoginHandler;
    }
    
    public UserAccountLoginOutput Login(UserAccountLoginQuery query)
    {
        return _userLoginHandler.Handle(query);
    }
}