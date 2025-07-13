using Application.Ingredient.query.getAllIngredient;
using Application.Utils;

namespace Application.Ingredient.query;

public class IngredientQueryProcessor
{
   private  readonly IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> _ingredientGetAllHandler;
   
   public IngredientQueryProcessor(IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> ingredientGetAllHandler)
      {
         _ingredientGetAllHandler = ingredientGetAllHandler;
      }
   
      public IngredientGetAllOutput GetAllIngredients(IngredientGetAllQuery query)
      {
         return _ingredientGetAllHandler.Handle(query);
      }
      
}