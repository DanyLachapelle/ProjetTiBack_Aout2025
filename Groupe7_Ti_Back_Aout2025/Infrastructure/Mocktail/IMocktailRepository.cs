using Domain;

namespace Infrastructure.Mocktail;

public interface IMocktailRepository
{
    Task<IEnumerable<Domain.Mocktail>> GetAllAsync();
    Task<Domain.Mocktail?> GetByIdAsync(int id);
    Task<Domain.Mocktail> CreateAsync(Domain.Mocktail mocktail);
    Task<Domain.Mocktail> UpdateAsync(Domain.Mocktail mocktail);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Domain.Ingredient>> GetAllIngredientsAsync();
} 