using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktails.query.getAllMocktail;

public class GetAllMocktailHandler:IQueryHandler<GetAllMocktailQuery, List<MocktailDto>>
{
    public readonly IMocktailRepository _mocktailRepository;
    
    public GetAllMocktailHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }
    public List<MocktailDto> Handle(GetAllMocktailQuery query)
    {
        var mocktails = _mocktailRepository.GetAllMocktails();

        if (mocktails == null || !mocktails.Any())
            return new List<MocktailDto>();

        return mocktails.Select(m => new MocktailDto
        {
            Id = m.id,
            Name = m.name,
            Description = m.description,
            Price = m.price,
            Available = IsAvailable(m),
            Image = m.image,
            Ingredients = m.MocktailIngredients.Select(mi => new MocktailIngredientDto
            {
                Name = mi.Ingredient.name,
                Quantity = mi.quantity,
                Unit = mi.unit
            }).ToList()
        }).ToList();
    }


    private bool IsAvailable(mocktail mocktail)
    {
        return true;
    }
    
}