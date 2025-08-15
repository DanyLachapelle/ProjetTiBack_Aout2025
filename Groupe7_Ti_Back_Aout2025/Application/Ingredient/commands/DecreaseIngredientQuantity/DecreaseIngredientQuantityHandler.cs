using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.DecreaseIngredientQuantity;

// Handler for decreasing ingredient quantity
public class DecreaseIngredientQuantityHandler : ICommandHandler<DecreaseIngredientQuantityCommand, DecreaseIngredientQuantityOutput>
{
    // Repository for accessing ingredient data
    private readonly IIngredientRepository _ingredientRepository;
    
    // Dependency injection of the repository
    public DecreaseIngredientQuantityHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Main command handling method
    public DecreaseIngredientQuantityOutput Handle(DecreaseIngredientQuantityCommand command)
    {
        // Quantity validation
        if (command.Quantity <= 0)
            throw new ArgumentException("Amount to decrease must be positive");

        // Retrieve the ingredient
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found");

        // Business operation: quantity decrease
        ingredient.DecreaseQuantity(command.Quantity);
        
        // Database update
        _ingredientRepository.DecreaseQuantity(ingredient);
       
        // Return the result
        return new DecreaseIngredientQuantityOutput
        {
            Success = true,
            Quantity = ingredient.Quantity,
            Message = "Quantity decreased successfully"
        };
    }
}