using System;
using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.UpdateLimitIngredient;

// Handler pour la mise à jour du seuil de réapprovisionnement
public class UpdateLimitIngredientHandler : ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput>
{
    // Répository pour la persistance des données
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection de dépendance du repository
    public UpdateLimitIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    // Méthode principale de traitement
    public UpdateLimitIngredientOutput Handle(UpdateLimitIngredientCommand command)
    {
        // Validation de la commande
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Command cannot be null");
        }

        // Validation de l'ID
        if (command.Id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID", nameof(command.Id));
        }

        // Validation du seuil de réapprovisionnement
        if (command.RestockThreshold < 0)
        {
            throw new ArgumentException("Restock threshold must be non-negative", nameof(command.RestockThreshold));
        }

        // Appel au repository pour la mise à jour
        var success = _ingredientRepository.UpdateRestockThreshold(command.Id, command.RestockThreshold);

        // Retour du résultat
        return new UpdateLimitIngredientOutput
        {
            Success = success,
            Message = success ? "Restock threshold updated successfully." : "Failed to update restock threshold."
        };
    }
}