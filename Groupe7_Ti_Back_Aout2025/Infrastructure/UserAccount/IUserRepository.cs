using Domain;

namespace Infrastructure.User;

public interface IUserRepository
{
    Domain.UserAccount GetUserByPseudo(string pseudo);
    void Save(Domain.UserAccount userAccount);
    Task<UserAccount?> GetUserByEmailAsync(string email);
    Task UpdatePasswordAsync(int userId, string newPassword);
}