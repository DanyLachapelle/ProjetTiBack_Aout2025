using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.DecreaseIngredientQuantity;

public class DecreaseIngredientQuantityHandler: ICommandHandler<DecreaseIngredientQuantityCommand,DecreaseIngredientQuantityOutput>
{
    private readonly IIngredientRepository _ingredientRepository;
    
public DecreaseIngredientQuantityHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public DecreaseIngredientQuantityOutput Handle(DecreaseIngredientQuantityCommand command)
    {
        if (command.quantity <= 0)
            throw new ArgumentException("Amount to decrease must be positive");

        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found");

        ingredient.DecreaseQuantity(command.quantity);
        _ingredientRepository.DecreaseQuantity(ingredient);
       

        return new DecreaseIngredientQuantityOutput
        {
            Success = true,
            quantity = ingredient.quantity,
            Message = "Quantity decreased successfully"
        };
    }
    
    

}