using System;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.commands.createMocktail;

// Handler for creating non-alcoholic cocktails (mocktails)
public class CreateMocktailHandler : ICommandHandler<CreateMocktailCommand, CreateMocktailOutput>
{
    // Repository for mocktail persistence
    private readonly IMocktailRepository _mocktailRepository;
    
    // Repository dependency injection
    public CreateMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    // Main command processing method
    public CreateMocktailOutput Handle(CreateMocktailCommand command)
    {
        // Command validation
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Creating base Mocktail entity
        var mocktail = new Mocktail
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Image = command.Image
        };

        // Processing ingredients
        foreach (var ingredientDto in command.Ingredients)
        {
            // Checking if ingredient exists
            var existingIngredient = _mocktailRepository.GetIngredientByName(ingredientDto.Name);

            Domain.Ingredient ingredientEntity;
            
            // Creating ingredient if it doesn't exist
            if (existingIngredient == null)
            {
                var newIngredient = new Domain.Ingredient
                {
                    Name = ingredientDto.Name,
                    Quantity = 0, // Initialized to 0 (will be managed separately)
                    Unit = ingredientDto.Unit,
                    RestockThreshold = 0
                };
                ingredientEntity = _mocktailRepository.AddIngredient(newIngredient);
            }
            else
            {
                ingredientEntity = existingIngredient;
            }

            // Creating many-to-many relationship with quantity
            var mocktailIngredient = new mocktail_ingredient
            {
                Ingredient = ingredientEntity,
                Quantity = ingredientDto.Quantity,
                Unit = ingredientDto.Unit
            };

            mocktail.MocktailIngredients.Add(mocktailIngredient);
        }

        // Persisting the complete mocktail
        var createdMocktail = _mocktailRepository.CreateMocktail(mocktail);

        // Returning created information
        return new CreateMocktailOutput
        {
            Id = createdMocktail.Id,
            Nom = createdMocktail.Name
        };
    }
}