using System.Linq;
using Application.DTOs;
using Application.Mocktails.query.getbyidMocktail;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.Query.GetByIdMocktail
{
    public class GetbyidMocktailHandler:IQueryHandler<GetbyidMocktailQuery, MocktailDto?>
    {
        private readonly IMocktailRepository _mocktailRepository;

        public GetbyidMocktailHandler(IMocktailRepository mocktailRepository)
        {
            _mocktailRepository = mocktailRepository;
        }

        public MocktailDto? Handle(GetbyidMocktailQuery query)
        {
            var mocktail = _mocktailRepository.GetMocktailById(query.Id);

            if (mocktail == null)
                return null;

            return new MocktailDto
            {
                Id = mocktail.Id,
                Name = mocktail.Name,
                Description = mocktail.Description,
                Price = mocktail.Price,
                Available = IsAvailable(mocktail),
                ForceAvailable = mocktail.ForceAvailable, // Laisser les valeurs null telles quelles
                Image = mocktail.Image,
                Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
                {
                    Name = mi.Ingredient.Name,
                    Quantity = mi.Quantity,
                    Unit = mi.Unit,
                    Allergen = mi.Ingredient.Allergen ?? "none" // Ajout de l'allergène
                }).ToList()
            };
        }

        private bool IsAvailable(Mocktail mocktail)
        {
            // Si forceAvailable est explicitement false, le mocktail est forcé indisponible
            if (mocktail.ForceAvailable == false)
                return false;
                
            // Si forceAvailable est true, le mocktail est toujours disponible
            if (mocktail.ForceAvailable == true)
                return true;
                
            // Si forceAvailable est null, vérifier le stock des ingrédients
            return mocktail.MocktailIngredients.All(mi => 
                mi.Ingredient.Quantity >= mi.Quantity
            );
        }
    }
}