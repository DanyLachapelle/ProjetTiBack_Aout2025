using System.Linq;
using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.updateMocktail;

// Handler for updating a mocktail
public class UpdateMocktailHandler : ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput>
{
    // Repository for data access
    public readonly IMocktailRepository _mocktailRepository;
    
    // Dependency injection
    public UpdateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    public UpdateMocktailOutput Handle(UpdateMocktailCommand command)
    {
        // Retrieving existing mocktail
        var mocktail = _mocktailRepository.GetMocktailById(command.Id);
        if (mocktail == null)
        {
            return new UpdateMocktailOutput
            {
                Success = false,
                Message = "Mocktail not found"
            };
        }

        // Updating basic properties
        mocktail.Name = command.Name;
        mocktail.Description = command.Description;
        mocktail.Price = command.Price;
        mocktail.Image = command.Image;
        mocktail.ForceAvailable = command.ForceAvailable;

        // --- Ingredients management ---
        
        // Synchronous retrieval of all ingredients
        var allIngredients = _mocktailRepository.GetAllIngredientsAsync().Result;

        // Removing old associations
        mocktail.MocktailIngredients.Clear();

        // Adding new ingredients
        foreach (var ingredientDto in command.Ingredients)
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.Name == ingredientDto.Name);
            if (ingredient == null)
            {
                // Error handling if ingredient not found
                return new UpdateMocktailOutput
                {
                    Success = false,
                    Message = $"Ingredient '{ingredientDto.Name}' not found"
                };
            }

            // Creating new association
            mocktail.MocktailIngredients.Add(new Domain.mocktail_ingredient
            {
                Mocktail = mocktail,
                Ingredient = ingredient,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            });
        }

        // Saving changes
        _mocktailRepository.UpdateMocktail(mocktail);

        return new UpdateMocktailOutput
        {
            Success = true,
            Message = "Mocktail updated successfully"
        };
    }
}