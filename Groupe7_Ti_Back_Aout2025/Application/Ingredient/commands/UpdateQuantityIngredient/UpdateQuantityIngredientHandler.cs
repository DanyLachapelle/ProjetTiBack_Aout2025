using System;
using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.UpdateQuantityIngredient;

// Handler pour la mise à jour de la quantité d'un ingrédient
public class UpdateQuantityIngredientHandler : ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput>
{
    // Répository pour l'accès aux données des ingrédients
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection de dépendance du repository
    public UpdateQuantityIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Méthode principale pour gérer la commande
    public UpdateQuantityIngredientOutput Handle(UpdateQuantityIngredientCommand command)
    {
        // Validation de la quantité à ajouter
        if (command.Amount <= 0)
            throw new ArgumentException("Amount to add must be positive");

        // Récupération de l'ingrédient
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found");

        // Mise à jour de la quantité (logique métier)
        ingredient.AddQuantity(command.Amount);
        
        // Persistance de la modification
        _ingredientRepository.UpdateQuantityIngredient(ingredient);

        // Retour du résultat avec la nouvelle quantité
        return new UpdateQuantityIngredientOutput
        {
            Success = true,
            NewQuantity = ingredient.Quantity,
            Message = "Quantity updated successfully"
        };
    }
}