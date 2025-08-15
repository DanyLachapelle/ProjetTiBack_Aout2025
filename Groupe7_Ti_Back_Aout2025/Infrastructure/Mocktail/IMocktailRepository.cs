using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Mocktail;

public interface IMocktailRepository
{
    //Task<IEnumerable<Domain.mocktail>> GetAllAsync();
    IEnumerable<Domain.Mocktail> GetAllMocktails();
    // Task<Domain.mocktail?> GetByIdAsync(int id);
    Domain.Mocktail? GetMocktailById(int id);
    //Task<Domain.mocktail> CreateAsync(Domain.mocktail mocktail);
    Domain.Mocktail CreateMocktail(Domain.Mocktail mocktail);
    //Task<Domain.mocktail> UpdateAsync(Domain.mocktail mocktail);
    Domain.Mocktail UpdateMocktail(Domain.Mocktail mocktail);
    //Task DeleteAsync(int id);
    void DeleteMocktail(Domain.Mocktail mocktail);

    Task<bool> ExistsAsync(int id);
    Domain.Ingredient? GetIngredientByName(string name);
    Domain.Ingredient AddIngredient(Domain.Ingredient ingredient);
    Task<IEnumerable<Domain.Ingredient>> GetAllIngredientsAsync();
} 