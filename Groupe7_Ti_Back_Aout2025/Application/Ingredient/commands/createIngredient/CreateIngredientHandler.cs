using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.createIngredient;

// Handler pour la commande de création d'ingrédient
public class CreateIngredientHandler:ICommandHandler<CreateIngredientCommand, CreateIngredientOutput>
{
    // Répository pour la persistance des ingrédients
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection de dépendance du repository
    public CreateIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    // Méthode principale pour gérer la commande
    public CreateIngredientOutput Handle(CreateIngredientCommand command)
    {
        // Création d'un nouvel ingrédient à partir de la commande
        var ingredient = new Domain.Ingredient()
        {
            Name = command.Name,
            Quantity = command.Quantity,
            RestockThreshold = command.RestockThreshold,
            Unit = command.Unit,
            Allergen = command.Allergen
        };

        // Persistance de l'ingrédient
        _ingredientRepository.CreateIngredient(ingredient);

        // Retour des données créées
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