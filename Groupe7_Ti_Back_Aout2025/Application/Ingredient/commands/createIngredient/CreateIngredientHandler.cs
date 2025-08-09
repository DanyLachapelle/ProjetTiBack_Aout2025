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
        // Valider et nettoyer l'allergène
        var allergen = string.IsNullOrWhiteSpace(command.allergen) ? "none" : command.allergen.ToLower().Trim();
        
        // Vérifier que l'allergène est valide
        if (!Domain.Ingredient.ValidAllergens.Contains(allergen))
        {
            allergen = "none"; // Valeur par défaut si invalide
        }

        var ingredient = new Domain.Ingredient()
        {
            name = command.name,
            quantity = command.quantity,
            restock_threshold = command.restock_threshold,
            unit = command.unit,
            allergen = allergen
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