using System;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.createMocktail;

// Handler pour la création de cocktails sans alcool (mocktails)
public class CreateMocktailHandler : ICommandHandler<CreateMocktailCommand, CreateMocktailOutput>
{
    // Répository pour la persistance des mocktails
    private readonly IMocktailRepository _mocktailRepository;
    
    // Injection de dépendance du repository
    public CreateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    // Méthode principale de traitement de la commande
    public CreateMocktailOutput Handle(CreateMocktailCommand command)
    {
        // Validation de la commande
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Création de l'entité Mocktail de base
        var mocktail = new Mocktail
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Image = command.Image
        };

        // Traitement des ingrédients
        foreach (var ingredientDto in command.Ingredients)
        {
            // Vérification de l'existence de l'ingrédient
            var existingIngredient = _mocktailRepository.GetIngredientByName(ingredientDto.Name);

            Domain.Ingredient ingredientEntity;
            
            // Création si l'ingrédient n'existe pas
            if (existingIngredient == null)
            {
                var newIngredient = new Domain.Ingredient
                {
                    Name = ingredientDto.Name,
                    Quantity = 0, // Initialisé à 0 (sera géré séparément)
                    Unit = ingredientDto.Unit,
                    RestockThreshold = 0
                };
                ingredientEntity = _mocktailRepository.AddIngredient(newIngredient);
            }
            else
            {
                ingredientEntity = existingIngredient;
            }

            // Création de la relation many-to-many avec quantité
            var mocktailIngredient = new mocktail_ingredient
            {
                Ingredient = ingredientEntity,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            };

            mocktail.MocktailIngredients.Add(mocktailIngredient);
        }

        // Persistance du mocktail complet
        var createdMocktail = _mocktailRepository.CreateMocktail(mocktail);

        // Retour des informations créées
        return new CreateMocktailOutput
        {
            Id = createdMocktail.Id,
            Nom = createdMocktail.Name
        };
    }
}