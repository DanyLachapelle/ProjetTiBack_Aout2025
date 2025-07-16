using Domain;

namespace Infrastructure.Mocktail;

public interface IMocktailRepository
{
    Task<IEnumerable<Domain.mocktail>> GetAllAsync();
    // Task<Domain.mocktail?> GetByIdAsync(int id);
    Domain.mocktail? GetMocktailById(int id);
    Task<Domain.mocktail> CreateAsync(Domain.mocktail mocktail);
    Task<Domain.mocktail> UpdateAsync(Domain.mocktail mocktail);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Domain.ingredient>> GetAllIngredientsAsync();
} 