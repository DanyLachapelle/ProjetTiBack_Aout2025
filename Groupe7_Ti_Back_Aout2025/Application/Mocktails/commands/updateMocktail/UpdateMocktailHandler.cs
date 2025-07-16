using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.updateMocktail;

public class UpdateMocktailHandler:ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput>
{
    public readonly IMocktailRepository _mocktailRepository;
    
    public UpdateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }
    public UpdateMocktailOutput Handle(UpdateMocktailCommand command)
    {
        var mocktail = _mocktailRepository.GetMocktailById(command.id);
        if (mocktail == null)
        {
            return new UpdateMocktailOutput
            {
                Success = false,
                Message = "Mocktail not found"
            };
        }

        // Mise à jour des propriétés classiques
        mocktail.nom = command.nom;
        mocktail.description = command.description;
        mocktail.prix = command.prix;
        mocktail.image = command.image;

        // --- Mise à jour des ingrédients ---

        // Récupérer tous les ingrédients en base
        var allIngredients = _mocktailRepository.GetAllIngredientsAsync().Result; // Utiliser .Result pour sync

        // Vider les anciennes associations
        mocktail.MocktailIngredients.Clear();

        foreach (var ingredientDto in command.Ingredients)
        {
            // Trouver l'ingrédient existant en base
            var ingredient = allIngredients.FirstOrDefault(i => i.name == ingredientDto.Name);
            if (ingredient == null)
            {
                // Option : ajouter un nouvel ingrédient ou retourner une erreur
                return new UpdateMocktailOutput
                {
                    Success = false,
                    Message = $"Ingredient '{ingredientDto.Name}' not found"
                };
            }

            var mocktailIngredient = new Domain.mocktail_ingredient
            {
                Mocktail = mocktail,
                Ingredient = ingredient,
                quantite = ingredientDto.Quantity,
                unite = ingredientDto.Unit
            };

            mocktail.MocktailIngredients.Add(mocktailIngredient);
        }

        // Enregistrer les modifications
        _mocktailRepository.UpdateMocktail(mocktail);

        return new UpdateMocktailOutput
        {
            Success = true,
            Message = "Mocktail updated successfully"
        };
    }

}