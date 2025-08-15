using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.DecreaseIngredientQuantity;

// Handler pour la diminution de quantité d'ingrédient
public class DecreaseIngredientQuantityHandler : ICommandHandler<DecreaseIngredientQuantityCommand, DecreaseIngredientQuantityOutput>
{
    // Répository pour l'accès aux données des ingrédients
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection de dépendance du repository
    public DecreaseIngredientQuantityHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Méthode principale de traitement de la commande
    public DecreaseIngredientQuantityOutput Handle(DecreaseIngredientQuantityCommand command)
    {
        // Validation de la quantité
        if (command.Quantity <= 0)
            throw new ArgumentException("Amount to decrease must be positive");

        // Récupération de l'ingrédient
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
            throw new ArgumentException("Ingredient not found");

        // Opération métier : diminution de la quantité
        ingredient.DecreaseQuantity(command.Quantity);
        
        // Mise à jour en base de données
        _ingredientRepository.DecreaseQuantity(ingredient);
       
        // Retour du résultat
        return new DecreaseIngredientQuantityOutput
        {
            Success = true,
            Quantity = ingredient.Quantity,
            Message = "Quantity decreased successfully"
        };
    }
}