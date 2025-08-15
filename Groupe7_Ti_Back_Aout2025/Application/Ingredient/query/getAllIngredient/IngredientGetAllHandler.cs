using System.Linq;
using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;

namespace Application.Ingredient.query.getAllIngredient;

// Handler pour la récupération de tous les ingrédients
public class IngredientGetAllHandler : IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput>
{
    // Répository pour l'accès aux données
    private readonly IIngredientRepository _ingredientRepository;
    
    // Injection de dépendance
    public IngredientGetAllHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    // Méthode principale de traitement
    public IngredientGetAllOutput Handle(IngredientGetAllQuery request)
    {
        // Récupération de tous les ingrédients depuis le repository
        var ingredients = _ingredientRepository.GetAllIngredient();

        // Transformation des entités en DTOs
        var ingredientDtos = ingredients.Select(i => new IngredientDto
        {
            Id = i.Id,
            Name = i.Name,
            Quantity = i.Quantity,
            RestockThreshold = i.RestockThreshold,
            Unit = i.Unit,
            Allergen = i.Allergen ?? "none", // Valeur par défaut pour les allergènes null
            LastModifiedAt = i.LastModifiedAt
        }).ToList();

        // Retour des résultats
        return new IngredientGetAllOutput
        {
            Ingredients = ingredientDtos
        };
    }
}