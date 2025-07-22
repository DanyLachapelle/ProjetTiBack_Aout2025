using System;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.createMocktail;

public class CreateMocktailHandler:ICommandHandler<CreateMocktailCommand, CreateMocktailOutput>
{
    private readonly IMocktailRepository _mocktailRepository;
    
    public CreateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }
    public CreateMocktailOutput Handle(CreateMocktailCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Création du mocktail
        var mocktail = new mocktail
        {
            name = command.name,
            description = command.description,
            price = command.price,
            image = command.image
        };

        // Ajouter les ingrédients
        foreach (var ingredientDto in command.Ingredients)
        {
            // Vérifie si l'ingrédient existe déjà
            var existingIngredient = _mocktailRepository.GetIngredientByName(ingredientDto.Name);

            Domain.Ingredient ingredientEntity;
            if (existingIngredient == null)
            {
                // Si pas trouvé, on le crée
                var newIngredient = new Domain.Ingredient
                {
                    name = ingredientDto.Name,
                    quantity = 0, // ou une autre logique si nécessaire
                    unit = ingredientDto.Unit,
                    restock_threshold = 0
                };
                ingredientEntity = _mocktailRepository.AddIngredient(newIngredient);
            }
            else
            {
                ingredientEntity = existingIngredient;
            }

            // Association à la table pivot
            var mocktailIngredient = new mocktail_ingredient
            {
                Ingredient = ingredientEntity,
                quantity = ingredientDto.Quantity,
                unit = ingredientDto.Unit
            };

            mocktail.MocktailIngredients.Add(mocktailIngredient);
        }

        // Enregistrement du mocktail
        var createdMocktail = _mocktailRepository.CreateMocktail(mocktail);

        return new CreateMocktailOutput
        {
            id = createdMocktail.id,
            nom = createdMocktail.name
        };
    }
}