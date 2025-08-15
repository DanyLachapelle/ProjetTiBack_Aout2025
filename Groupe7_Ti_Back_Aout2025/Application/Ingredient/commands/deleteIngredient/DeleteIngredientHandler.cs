using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.deleteIngredient;

// Handler for ingredient deletion
public class DeleteIngredientHandler : ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput> 
{
    // Repository for data access
    private readonly IIngredientRepository _ingredientRepository;
    
    // Repository injection
    public DeleteIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Handles the delete command
    public DeleteIngredientOutput Handle(DeleteIngredientCommand command)
    {
        // Verify ingredient exists
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
        {
            // Return error if not found
            return new DeleteIngredientOutput
            {
                Success = false,
                Message = "Ingredient not found."
            };
        }

        // Perform deletion
        _ingredientRepository.DeleteIngredient(ingredient);

        // Return success confirmation
        return new DeleteIngredientOutput
        {
            Success = true,
            Message = "Ingredient deleted successfully."
        };
    }
}