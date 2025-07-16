using Domain;

namespace Infrastructure.Mocktail;

public interface IMocktailRepository
{
    Task<IEnumerable<Domain.mocktail>> GetAllAsync();
    // Task<Domain.mocktail?> GetByIdAsync(int id);
    Domain.mocktail? GetMocktailById(int id);
    //Task<Domain.mocktail> CreateAsync(Domain.mocktail mocktail);
    Domain.mocktail CreateMocktail(Domain.mocktail mocktail);
    Task<Domain.mocktail> UpdateAsync(Domain.mocktail mocktail);
    //Task DeleteAsync(int id);
    void DeleteMocktail(mocktail mocktail);

    Task<bool> ExistsAsync(int id);
    ingredient? GetIngredientByName(string name);
    ingredient AddIngredient(ingredient ingredient);
    Task<IEnumerable<Domain.ingredient>> GetAllIngredientsAsync();
} 