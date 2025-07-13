using Application.Ingredient.query.getAllIngredient;
using Application.Utils;

namespace Application.Ingredient.query;

public class IngredientGetAllQueryProcessor
{
   private  readonly IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> _ingredientGetAllHandler;
   
   public IngredientGetAllQueryProcessor(IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> ingredientGetAllHandler)
      {
         _ingredientGetAllHandler = ingredientGetAllHandler;
      }
   
      public IngredientGetAllOutput GetAllIngredients(IngredientGetAllQuery query)
      {
         return _ingredientGetAllHandler.Handle(query);
      }
      
}