namespace Infrastructure.User;

public class UserRepository:IUserRepository
{
    private UserContext _userContext;
    public UserRepository(UserContext userContext)
    {
        _userContext = userContext;
    }
    
    public Domain.User GetUserByPseudo(string pseudo)
    { 
        return _userContext.Users.FirstOrDefault(u => u.pseudo == pseudo); 
    }

    public void Save(Domain.User user)
    {
        _userContext.Users.Update(user);
        _userContext.SaveChanges();
    }
}