using Application.Utils;
using Infrastructure.Ingredient;

namespace Application.Ingredient.commands.deleteIngredient;

// Handler pour la suppression d'un ingrédient
public class DeleteIngredientHandler : ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput> 
{
    // Répository pour l'accès aux données
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection du repository
    public DeleteIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    // Traitement de la commande de suppression
    public DeleteIngredientOutput Handle(DeleteIngredientCommand command)
    {
        // Vérification de l'existence de l'ingrédient
        var ingredient = _ingredientRepository.GetIngredientById(command.Id);
        if (ingredient == null)
        {
            // Retour d'erreur si non trouvé
            return new DeleteIngredientOutput
            {
                Success = false,
                Message = "Ingredient not found."
            };
        }

        // Suppression effective
        _ingredientRepository.DeleteIngredient(ingredient);

        // Confirmation de suppression
        return new DeleteIngredientOutput
        {
            Success = true,
            Message = "Ingredient deleted successfully."
        };
    }
}