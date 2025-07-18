using Application.Utils;
using Infrastructure.Ingredient;


namespace Application.Ingredient.commands.UpdateLimitIngredient;

public class UpdateLimitIngredientHandler:ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput>
{
    private readonly IIngredientRepository _ingredientRepository;
    
    public UpdateLimitIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public UpdateLimitIngredientOutput Handle(UpdateLimitIngredientCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Command cannot be null");
        }

        if (command.Id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID", nameof(command.Id));
        }

        if (command.RestockThreshold < 0)
        {
            throw new ArgumentException("Restock threshold must be non-negative", nameof(command.RestockThreshold));
        }

        var success = _ingredientRepository.UpdateRestockThreshold(command.Id, command.RestockThreshold);

        return new UpdateLimitIngredientOutput
        {
            Success = success,
            Message = success ? "Restock threshold updated successfully." : "Failed to update restock threshold."
        };
    }
}