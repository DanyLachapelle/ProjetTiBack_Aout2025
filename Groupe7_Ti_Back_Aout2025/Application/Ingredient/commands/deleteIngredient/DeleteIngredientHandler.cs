using Application.Utils;
using Infrastructure.User.Ingredient;

namespace Application.Ingredient.commands.deleteIngredient;

public class DeleteIngredientHandler:ICommandHandler<DeleteIngredientQuery,DeleteIngredientOutput> 
{
    private readonly IIngredientRepository _ingredientRepository;
    
    public DeleteIngredientHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    public DeleteIngredientOutput Handle(DeleteIngredientQuery command)
    {
        var ingredient = _ingredientRepository.GetIngredientById(command.id);
        if (ingredient == null)
        {
            return new DeleteIngredientOutput
            {
                Success = false,
                Message = "Ingredient not found."
            };
        }

        _ingredientRepository.DeleteIngredient(ingredient);

        return new DeleteIngredientOutput
        {
            Success = true,
            Message = "Ingredient deleted successfully."
        };
    }
}