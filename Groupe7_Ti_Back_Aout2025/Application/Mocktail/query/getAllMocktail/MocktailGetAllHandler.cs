using System.Linq;
using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Mocktail;

namespace Application.Mocktail.query.getAllMocktail;

public class MocktailGetAllHandler: IQueryHandler<MocktailGetAllQuery, MocktailGetAllOutput>
{
    private readonly IMocktailRepository _mocktailRepository;
    
    public MocktailGetAllHandler(IMocktailRepository mocktailRepository)
    {
        _mocktailRepository = mocktailRepository;
    }
    
    public MocktailGetAllOutput Handle(MocktailGetAllQuery request)
    {
        var _mocktails = _mocktailRepository.GetAllMocktail();

        var _mocktailDtos = _mocktails.Select(i => new MocktailDto
        {
            Id = i.Id,
            Name = i.Name,
            Ingredients = i.MocktailIngredients,
            Image = i.Image,
            Price = i.Price,
            Available = i.Available,
            Description = i.Description
            
            
        }).ToList();

        return new MocktailGetAllOutput
        {
            Mocktails = _mocktailDtos
        };
    }

    
}