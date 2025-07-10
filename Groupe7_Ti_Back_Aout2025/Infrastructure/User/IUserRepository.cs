namespace Infrastructure.User;

public interface IUserRepository
{
    Domain.User GetUserByPseudo(string pseudo);
    void Save(Domain.User user);
}