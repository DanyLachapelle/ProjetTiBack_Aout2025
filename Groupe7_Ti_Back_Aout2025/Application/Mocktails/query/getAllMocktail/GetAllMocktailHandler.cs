using System.Collections.Generic;
using System.Linq;
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
            ForceAvailable = m.forceAvailable, // Laisser les valeurs null telles quelles
            Image = m.image,
            Ingredients = m.MocktailIngredients.Select(mi => new MocktailIngredientDto
            {
                Name = mi.Ingredient.name,
                Quantity = mi.quantity,
                Unit = mi.unit,
                Allergen = mi.Ingredient.allergen ?? "none" // Ajout de l'allergène
            }).ToList()
        }).ToList();
    }


    private bool IsAvailable(Mocktail mocktail)
    {
        // Si forceAvailable est explicitement false, le mocktail est forcé indisponible
        if (mocktail.forceAvailable == false)
            return false;
            
        // Si forceAvailable est true, le mocktail est toujours disponible
        if (mocktail.forceAvailable == true)
            return true;
            
        // Si forceAvailable est null, vérifier le stock des ingrédients
        return mocktail.MocktailIngredients.All(mi => 
            mi.Ingredient.quantity >= mi.quantity
        );
    }
    
}