using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.createIngredient;

public class CreateIngredientHandler:ICommandHandler<CreateIngredientCommand, CreateIngredientOutput>
{
    
    private readonly IIngredientRepository _ingredientRepository;
    
    public CreateIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public CreateIngredientOutput Handle(CreateIngredientCommand command)
    {
        var ingredient = new Domain.Ingredient()
        {
            name = command.name,
            quantity = command.quantity,
            restock_threshold = command.restock_threshold,
            unit = command.unit,
            allergen = command.allergen
        };

        _ingredientRepository.CreateIngredient(ingredient);

        return new CreateIngredientOutput
        {
            name = ingredient.name,
            quantity = ingredient.quantity,
            restock_threshold = ingredient.restock_threshold,
            unit = ingredient.unit,
            allergen = ingredient.allergen
        };
    }

}