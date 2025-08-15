using System.Collections.Generic;
using System.Linq;
using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.query.getAllMocktail;

// Handler for retrieving all mocktails with availability calculation
public class GetAllMocktailHandler : IQueryHandler<GetAllMocktailQuery, List<MocktailDto>>
{
    // Repository for mocktail data access
    public readonly IMocktailRepository _mocktailRepository;
    
    // Dependency injection
    public GetAllMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }

    // Main query handling method
    public List<MocktailDto> Handle(GetAllMocktailQuery query)
    {
        // Retrieve all mocktails from repository
        var mocktails = _mocktailRepository.GetAllMocktails();

        // Return empty list if no mocktails found
        if (mocktails == null || !mocktails.Any())
            return new List<MocktailDto>();

        // Transform domain models to DTOs
        return mocktails.Select(m => new MocktailDto
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            Price = m.Price,
            Available = IsAvailable(m), // Calculate availability
            ForceAvailable = m.ForceAvailable, // Preserve null values
            Image = m.Image,
            // Map ingredients with their details
            Ingredients = m.MocktailIngredients.Select(mi => new MocktailIngredientDto
            {
                Name = mi.Ingredient.Name,
                Quantity = mi.Quantity,
                Unit = mi.Unit,
                Allergen = mi.Ingredient.Allergen ?? "none" // Default allergen value
            }).ToList()
        }).ToList();
    }

    // Helper method to determine mocktail availability
    private bool IsAvailable(Mocktail mocktail)
    {
        
        // Explicitly forced unavailable
        if (mocktail.ForceAvailable == false)
            return false;
            
        // Explicitly forced available    
        if (mocktail.ForceAvailable == true)
            return true;
            
        // Check stock for all ingredients if no force flag
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.Quantity >= mi.Quantity
        );
    }
}