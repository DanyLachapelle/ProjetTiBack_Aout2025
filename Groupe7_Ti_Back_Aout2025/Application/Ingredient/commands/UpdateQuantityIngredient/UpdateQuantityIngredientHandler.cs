using System;
using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.UpdateQuantityIngredient;

// Handler for updating ingredient quantity
public class UpdateQuantityIngredientHandler : ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput>
{
    // Repository for ingredient data access
    private readonly IIngredientRepository _ingredientRepository;
    
    // Dependency injection of the repository
    public UpdateQuantityIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Main command handling method
    public UpdateQuantityIngredientOutput Handle(UpdateQuantityIngredientCommand command)
    {
        // Validate the amount to add
        if (command.Amount <= 0)
            throw new ArgumentException("Amount to add must be positive", nameof(command.Amount));

        // Retrieve the ingredient
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found", nameof(command.Id));

        // Business logic: update quantity
        ingredient.AddQuantity(command.Amount);
        
        // Persist the changes
        _ingredientRepository.UpdateQuantityIngredient(ingredient);

        // Return result with new quantity
        return new UpdateQuantityIngredientOutput
        {
            Success = true,
            NewQuantity = ingredient.Quantity,
            Message = "Quantity updated successfully"
        };
    }
}