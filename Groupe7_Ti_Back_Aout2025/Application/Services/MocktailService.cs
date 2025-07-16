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

    // public async Task<MocktailDto?> GetByIdAsync(int id)
    // {
    //     var mocktail = await _mocktailRepository.GetMocktailById(id);
    //     return mocktail != null ? MapToDto(mocktail) : null;
    // }
    // public MocktailDto? GetMocktailById(int id)
    // {
    //     var mocktail = _mocktailRepository.GetMocktailById(id);
    //     return mocktail != null ? MapToDto(mocktail) : null;
    // }

    public async Task<MocktailDto> CreateAsync(CreateMocktailRequest request)
    {
        // Récupérer tous les ingrédients pour les mapper
        var allIngredients = await _mocktailRepository.GetAllIngredientsAsync();
        
        // Créer le mocktail
        var mocktail = new Domain.mocktail
        {
            nom = request.Name,
            description = request.Description,
            prix = request.Price,
            image = request.Image
        };

        // Créer les associations mocktail-ingrédients
        var mocktailIngredients = new List<Domain.mocktail_ingredient>();
        
        foreach (var ingredientRequest in request.Ingredients)
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.name == ingredientRequest.Name);
            if (ingredient == null)
            {
                throw new ArgumentException($"Ingredient '{ingredientRequest.Name}' not found");
            }

            var mocktailIngredient = new Domain.mocktail_ingredient()
            {
                Mocktail = mocktail,
                Ingredient = ingredient,
                quantite = ingredientRequest.Quantity,
                unite = ingredientRequest.Unit
            };
            
            mocktailIngredients.Add(mocktailIngredient);
        }

        mocktail.MocktailIngredients = mocktailIngredients;

        // Sauvegarder le mocktail
        var createdMocktail = await _mocktailRepository.CreateAsync(mocktail);
        
        return MapToDto(createdMocktail);
    }

    // public async Task<MocktailDto> UpdateAsync(int id, UpdateMocktailRequest request)
    // {
    //     // Récupérer le mocktail existant
    //     var existingMocktail = await _mocktailRepository.GetMocktailById(id);
    //     if (existingMocktail == null)
    //     {
    //         throw new ArgumentException($"Mocktail with id {id} not found");
    //     }
    //
    //     // Récupérer tous les ingrédients pour les mapper
    //     var allIngredients = await _mocktailRepository.GetAllIngredientsAsync();
    //     
    //     // Mettre à jour les propriétés du mocktail
    //     existingMocktail.nom = request.Name;
    //     existingMocktail.description = request.Description;
    //     existingMocktail.prix = request.Price;
    //     existingMocktail.image = request.Image;
    //
    //     // Supprimer les anciennes associations d'ingrédients
    //     existingMocktail.MocktailIngredients.Clear();
    //
    //     // Créer les nouvelles associations mocktail-ingrédients
    //     var mocktailIngredients = new List<Domain.mocktail_ingredient>();
    //     
    //     foreach (var ingredientRequest in request.Ingredients)
    //     {
    //         var ingredient = allIngredients.FirstOrDefault(i => i.name == ingredientRequest.Name);
    //         if (ingredient == null)
    //         {
    //             throw new ArgumentException($"Ingredient '{ingredientRequest.Name}' not found");
    //         }
    //
    //         var mocktailIngredient = new Domain.mocktail_ingredient()
    //         {
    //             Mocktail = existingMocktail,
    //             Ingredient = ingredient,
    //             quantite = ingredientRequest.Quantity,
    //             unite = ingredientRequest.Unit
    //         };
    //         
    //         mocktailIngredients.Add(mocktailIngredient);
    //     }
    //
    //     existingMocktail.MocktailIngredients = mocktailIngredients;
    //
    //     // Sauvegarder les modifications
    //     var updatedMocktail = await _mocktailRepository.UpdateAsync(existingMocktail);
    //     
    //     return MapToDto(updatedMocktail);
    // }
    // public MocktailDto Update(int id, UpdateMocktailRequest request)
    // {
    //     // Récupérer le mocktail existant
    //     var existingMocktail = _mocktailRepository.GetMocktailById(id);
    //     if (existingMocktail == null)
    //     {
    //         throw new ArgumentException($"Mocktail with id {id} not found");
    //     }
    //
    //     // Récupérer tous les ingrédients
    //     var allIngredients = _mocktailRepository.GetAllIngredientsAsync();
    //
    //     // Mettre à jour les propriétés du mocktail
    //     existingMocktail.nom = request.Name;
    //     existingMocktail.description = request.Description;
    //     existingMocktail.prix = request.Price;
    //     existingMocktail.image = request.Image;
    //
    //     // Supprimer les anciennes associations
    //     existingMocktail.MocktailIngredients.Clear();
    //
    //     // Créer les nouvelles associations
    //     var mocktailIngredients = new List<Domain.mocktail_ingredient>();
    //     foreach (var ingredientRequest in request.Ingredients)
    //     {
    //         var ingredient = allIngredients.FirstOrDefault(i => i.name == ingredientRequest.Name);
    //         if (ingredient == null)
    //         {
    //             throw new ArgumentException($"Ingredient '{ingredientRequest.Name}' not found");
    //         }
    //
    //         var mocktailIngredient = new Domain.mocktail_ingredient()
    //         {
    //             Mocktail = existingMocktail,
    //             Ingredient = ingredient,
    //             quantite = ingredientRequest.Quantity,
    //             unite = ingredientRequest.Unit
    //         };
    //
    //         mocktailIngredients.Add(mocktailIngredient);
    //     }
    //
    //     existingMocktail.MocktailIngredients = mocktailIngredients;
    //
    //     // Sauvegarder les modifications
    //     var updatedMocktail = _mocktailRepository.UpdateAsync(existingMocktail);
    //
    //     return MapToDto(updatedMocktail);
    // }

    public async Task DeleteAsync(int id)
    {
        await _mocktailRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
    {
        var ingredients = await _mocktailRepository.GetAllIngredientsAsync();
        return ingredients.Select(MapToIngredientDto);
    }

    private MocktailDto MapToDto(Domain.mocktail mocktail)
    {
        return new MocktailDto
        {
            Id = mocktail.id,
            Name = mocktail.nom,
            Description = mocktail.description,
            Price = mocktail.prix,
            Available = IsAvailable(mocktail),
            Image = mocktail.image,
            Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
            {
                Name = mi.Ingredient.name,
                Quantity = mi.quantite,
                Unit = mi.unite
            }).ToList()
        };
    }

    private IngredientDto MapToIngredientDto(Domain.ingredient ingredient)
    {
        return new IngredientDto
        {
            id = ingredient.id,
            name = ingredient.name,
            quantity = ingredient.quantity,
            restock_threshold = ingredient.restock_threshold,
            unit = ingredient.unit
        };
    }

    private bool IsAvailable(Domain.mocktail mocktail)
    {
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.quantity >= mi.quantite
        );
    }
} 