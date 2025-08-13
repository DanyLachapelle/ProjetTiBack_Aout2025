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
        var mocktail = new Mocktail
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Image = command.Image
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
                    Name = ingredientDto.Name,
                    Quantity = 0, // ou une autre logique si nécessaire
                    Unit = ingredientDto.Unit,
                    RestockThreshold = 0
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
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            };

            mocktail.MocktailIngredients.Add(mocktailIngredient);
        }

        // Enregistrement du mocktail
        var createdMocktail = _mocktailRepository.CreateMocktail(mocktail);

        return new CreateMocktailOutput
        {
            Id = createdMocktail.Id,
            Nom = createdMocktail.Name
        };
    }
}