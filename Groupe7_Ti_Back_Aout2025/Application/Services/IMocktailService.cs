using Application.DTOs;

namespace Application.Services;

public interface IMocktailService
{
    Task<IEnumerable<MocktailDto>> GetAllAsync();
    //Task<MocktailDto?> GetByIdAsync(int id);
    //MocktailDto? GetMocktailById(int id);
    Task<MocktailDto> CreateAsync(CreateMocktailRequest request);
    //Task<MocktailDto> UpdateAsync(int id, UpdateMocktailRequest request);
    Task DeleteAsync(int id);
    Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
} 