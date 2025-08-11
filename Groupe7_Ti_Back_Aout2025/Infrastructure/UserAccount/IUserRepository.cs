using Domain;

namespace Infrastructure.User;

public interface IUserRepository
{
    Domain.User_account GetUserByPseudo(string pseudo);
    void Save(Domain.User_account userAccount);
    Task<User_account?> GetUserByEmailAsync(string email);
    Task UpdatePasswordAsync(int userId, string newPassword);
}