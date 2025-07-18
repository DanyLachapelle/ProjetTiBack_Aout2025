using System;
using Application.Utils;
using Infrastructure.Ingredient;


namespace Application.Ingredient.commands.UpdateQuantityIngredient;

public class UpdateQuantityIngredientHandler:ICommandHandler<UpdateQuantityIngredientQuery, UpdateQuantityIngredientOutput>
{
    private readonly IIngredientRepository _ingredientRepository;
    
public UpdateQuantityIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    public UpdateQuantityIngredientOutput Handle(UpdateQuantityIngredientQuery command)
    {
        if (command.Amount <= 0)
            throw new ArgumentException("Amount to add must be positive");

        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found");

        ingredient.AddQuantity(command.Amount);
        _ingredientRepository.UpdateQuantityIngredient(ingredient);

        return new UpdateQuantityIngredientOutput
        {
            Success = true,
            NewQuantity = ingredient.quantity,
            Message = "Quantity updated successfully"
        };
    }
}