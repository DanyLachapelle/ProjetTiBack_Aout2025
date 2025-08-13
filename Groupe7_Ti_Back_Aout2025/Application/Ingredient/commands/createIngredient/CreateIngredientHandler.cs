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
            Name = command.Name,
            Quantity = command.Quantity,
            RestockThreshold = command.RestockThreshold,
            Unit = command.Unit,
            Allergen = command.Allergen
        };

        _ingredientRepository.CreateIngredient(ingredient);

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