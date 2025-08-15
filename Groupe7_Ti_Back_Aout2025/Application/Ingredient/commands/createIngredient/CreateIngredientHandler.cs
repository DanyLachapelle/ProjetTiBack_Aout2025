using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.createIngredient;

// Handler for the ingredient creation command
public class CreateIngredientHandler : ICommandHandler<CreateIngredientCommand, CreateIngredientOutput>
{
    // Repository for ingredient persistence
    private readonly IIngredientRepository _ingredientRepository;
    
    // Dependency injection of the repository
    public CreateIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    // Main method to handle the command
    public CreateIngredientOutput Handle(CreateIngredientCommand command)
    {
        // Create a new ingredient from the command
        var ingredient = new Domain.Ingredient()
        {
            Name = command.Name,
            Quantity = command.Quantity,
            RestockThreshold = command.RestockThreshold,
            Unit = command.Unit,
            Allergen = command.Allergen
        };

        // Persist the ingredient
        _ingredientRepository.CreateIngredient(ingredient);

        // Return the created data
        return new CreateIngredientOutput
        {
            Name = ingredient.Name,
            Quantity = ingredient.Quantity,
            RestockThreshold = ingredient.RestockThreshold,
            Unit = ingredient.Unit,
            Allergen = ingredient.Allergen
        };
    }
}