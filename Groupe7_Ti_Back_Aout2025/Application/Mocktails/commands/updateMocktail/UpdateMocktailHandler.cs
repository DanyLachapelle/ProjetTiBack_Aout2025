using System.Linq;
using Application.Utils;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.updateMocktail;

// Handler pour la mise à jour d'un mocktail
public class UpdateMocktailHandler : ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput>
{
    // Répository pour l'accès aux données
    public readonly IMocktailRepository _mocktailRepository;
    
    // Injection de dépendance
    public UpdateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    public UpdateMocktailOutput Handle(UpdateMocktailCommand command)
    {
        // Récupération du mocktail existant
        var mocktail = _mocktailRepository.GetMocktailById(command.Id);
        if (mocktail == null)
        {
            return new UpdateMocktailOutput
            {
                Success = false,
                Message = "Mocktail not found"
            };
        }

        // Mise à jour des propriétés de base
        mocktail.Name = command.Name;
        mocktail.Description = command.Description;
        mocktail.Price = command.Price;
        mocktail.Image = command.Image;
        mocktail.ForceAvailable = command.ForceAvailable;

        // --- Gestion des ingrédients ---
        
        // Récupération synchrone de tous les ingrédients
        var allIngredients = _mocktailRepository.GetAllIngredientsAsync().Result;

        // Suppression des anciennes associations
        mocktail.MocktailIngredients.Clear();

        // Ajout des nouveaux ingrédients
        foreach (var ingredientDto in command.Ingredients)
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.Name == ingredientDto.Name);
            if (ingredient == null)
            {
                // Gestion d'erreur si ingrédient non trouvé
                return new UpdateMocktailOutput
                {
                    Success = false,
                    Message = $"Ingredient '{ingredientDto.Name}' not found"
                };
            }

            // Création de la nouvelle association
            mocktail.MocktailIngredients.Add(new Domain.mocktail_ingredient
            {
                Mocktail = mocktail,
                Ingredient = ingredient,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            });
        }

        // Sauvegarde des modifications
        _mocktailRepository.UpdateMocktail(mocktail);

        return new UpdateMocktailOutput
        {
            Success = true,
            Message = "Mocktail updated successfully"
        };
    }
}