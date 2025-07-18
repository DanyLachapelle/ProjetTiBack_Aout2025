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
                Id = mocktail.id,
                Name = mocktail.name,
                Description = mocktail.description,
                Price = mocktail.price,
                Available = IsAvailable(mocktail),
                Image = mocktail.image,
                Ingredients = mocktail.MocktailIngredients.Select(mi => new MocktailIngredientDto
                {
                    Name = mi.Ingredient.name,
                    Quantity = mi.quantity,
                    Unit = mi.unit
                }).ToList()
            };
        }

        private bool IsAvailable(mocktail mocktail)
        {
            // ta logique de disponibilité ici
            return true;
        }
    }
}