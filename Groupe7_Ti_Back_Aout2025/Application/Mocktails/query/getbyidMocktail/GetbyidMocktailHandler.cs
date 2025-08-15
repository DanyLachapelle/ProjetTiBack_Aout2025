using System.Linq;
using Application.DTOs;
using Application.Mocktails.query.getbyidMocktail;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.Query.GetByIdMocktail
{
    // Handler for retrieving a single mocktail by ID
    public class GetbyidMocktailHandler : IQueryHandler<GetbyidMocktailQuery, MocktailDto?>
    {
        private readonly IMocktailRepository _mocktailRepository;

        // Dependency injection of mocktail repository
        public GetbyidMocktailHandler(IMocktailRepository mocktailRepository)
        {
            _mocktailRepository = mocktailRepository;
        }

        // Main handler method
        public MocktailDto? Handle(GetbyidMocktailQuery query)
        {
            // Fetch mocktail from repository
            var mocktail = _mocktailRepository.GetMocktailById(query.Id);

            // Return null if not found (nullable return type)
            if (mocktail == null)
                return null;

            // Map domain model to DTO
            return new MocktailDto
            {
                Id = mocktail.Id,
                Name = mocktail.Name,
                Description = mocktail.Description,
                Price = mocktail.Price,
                Available = IsAvailable(mocktail),  // Calculate availability
                ForceAvailable = mocktail.ForceAvailable,  // Preserve nullability
                Image = mocktail.Image,
                // Map nested ingredients
                Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
                {
                    Name = mi.Ingredient.Name,
                    Quantity = mi.Quantity,
                    Unit = mi.Unit,
                    Allergen = mi.Ingredient.Allergen ?? "none"  // Default value for null allergens
                }).ToList()
            };
        }

        // Availability calculation logic
        private bool IsAvailable(Mocktail mocktail)
        {
            // Forced unavailable
            if (mocktail.ForceAvailable == false)
                return false;
                
            // Forced available    
            if (mocktail.ForceAvailable == true)
                return true;
                
            // Stock-based availability check
            return mocktail.MocktailIngredients.All(mi => 
                mi.Ingredient.Quantity >= mi.Quantity
            );
        }
    }
}