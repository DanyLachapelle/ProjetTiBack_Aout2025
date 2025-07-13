namespace Infrastructure.User;

public interface IUserRepository
{
    Domain.UserAccount GetUserByPseudo(string pseudo);
    void Save(Domain.UserAccount userAccount);
}