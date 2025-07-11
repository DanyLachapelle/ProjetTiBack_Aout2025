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
        // Récupérer tous les ingrédients pour les mapper
        var allIngredients = await _mocktailRepository.GetAllIngredientsAsync();
        
        // Créer le mocktail
        var mocktail = new Domain.Mocktail
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Image = request.Image
        };

        // Créer les associations mocktail-ingrédients
        var mocktailIngredients = new List<Domain.MocktailIngredient>();
        
        foreach (var ingredientRequest in request.Ingredients)
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.Name == ingredientRequest.Name);
            if (ingredient == null)
            {
                throw new ArgumentException($"Ingredient '{ingredientRequest.Name}' not found");
            }

            var mocktailIngredient = new Domain.MocktailIngredient
            {
                Mocktail = mocktail,
                Ingredient = ingredient,
                Quantity = ingredientRequest.Quantity,
                Unit = ingredientRequest.Unit
            };
            
            mocktailIngredients.Add(mocktailIngredient);
        }

        mocktail.MocktailIngredients = mocktailIngredients;

        // Sauvegarder le mocktail
        var createdMocktail = await _mocktailRepository.CreateAsync(mocktail);
        
        return MapToDto(createdMocktail);
    }

    public async Task<MocktailDto> UpdateAsync(int id, UpdateMocktailRequest request)
    {
        // Récupérer le mocktail existant
        var existingMocktail = await _mocktailRepository.GetByIdAsync(id);
        if (existingMocktail == null)
        {
            throw new ArgumentException($"Mocktail with id {id} not found");
        }

        // Récupérer tous les ingrédients pour les mapper
        var allIngredients = await _mocktailRepository.GetAllIngredientsAsync();
        
        // Mettre à jour les propriétés du mocktail
        existingMocktail.Name = request.Name;
        existingMocktail.Description = request.Description;
        existingMocktail.Price = request.Price;
        existingMocktail.Image = request.Image;

        // Supprimer les anciennes associations d'ingrédients
        existingMocktail.MocktailIngredients.Clear();

        // Créer les nouvelles associations mocktail-ingrédients
        var mocktailIngredients = new List<Domain.MocktailIngredient>();
        
        foreach (var ingredientRequest in request.Ingredients)
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.Name == ingredientRequest.Name);
            if (ingredient == null)
            {
                throw new ArgumentException($"Ingredient '{ingredientRequest.Name}' not found");
            }

            var mocktailIngredient = new Domain.MocktailIngredient
            {
                Mocktail = existingMocktail,
                Ingredient = ingredient,
                Quantity = ingredientRequest.Quantity,
                Unit = ingredientRequest.Unit
            };
            
            mocktailIngredients.Add(mocktailIngredient);
        }

        existingMocktail.MocktailIngredients = mocktailIngredients;

        // Sauvegarder les modifications
        var updatedMocktail = await _mocktailRepository.UpdateAsync(existingMocktail);
        
        return MapToDto(updatedMocktail);
    }

    public async Task DeleteAsync(int id)
    {
        await _mocktailRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
    {
        var ingredients = await _mocktailRepository.GetAllIngredientsAsync();
        return ingredients.Select(MapToIngredientDto);
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

    private IngredientDto MapToIngredientDto(Domain.Ingredient ingredient)
    {
        return new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Stock = ingredient.Stock,
            Limit = ingredient.Limit,
            Unit = ingredient.Unit
        };
    }

    private bool IsAvailable(Domain.Mocktail mocktail)
    {
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.Stock >= mi.Quantity
        );
    }
} 