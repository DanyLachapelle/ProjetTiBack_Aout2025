using Domain;
using Application.DTOs;
using Infrastructure.Mocktail;

namespace Application.Services;

public class MocktailService : IMocktailService
{
    private readonly IMocktailRepository _mocktailRepository;

    public MocktailService(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    public async Task<IEnumerable<MocktailDto>> GetAllAsync()
    {
        var mocktails = await _mocktailRepository.GetAllAsync();
        return mocktails.Select(MapToDto);
    }

    public async Task<MocktailDto?> GetByIdAsync(int id)
    {
        var mocktail = await _mocktailRepository.GetByIdAsync(id);
        return mocktail != null ? MapToDto(mocktail) : null;
    }

    public async Task<MocktailDto> CreateAsync(CreateMocktailRequest request)
    {
        // TODO: Implémenter la création
        throw new NotImplementedException();
    }

    public async Task<MocktailDto> UpdateAsync(int id, UpdateMocktailRequest request)
    {
        // TODO: Implémenter la mise à jour
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        await _mocktailRepository.DeleteAsync(id);
    }

    private MocktailDto MapToDto(Domain.Mocktail mocktail)
    {
        return new MocktailDto
        {
            Id = mocktail.Id,
            Name = mocktail.Name,
            Description = mocktail.Description,
            Price = mocktail.Price,
            Available = IsAvailable(mocktail),
            Image = mocktail.Image,
            Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
            {
                Name = mi.Ingredient.Name,
                Quantity = mi.Quantity,
                Unit = mi.Unit
            }).ToList()
        };
    }

    private bool IsAvailable(Domain.Mocktail mocktail)
    {
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.Stock >= mi.Quantity
        );
    }
} 