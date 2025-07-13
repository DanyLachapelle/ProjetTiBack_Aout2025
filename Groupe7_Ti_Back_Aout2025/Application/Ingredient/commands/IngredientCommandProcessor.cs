using Application.Ingredient.commands.createIngredient;
using Application.Utils;

namespace Application.Ingredient.commands;

public class IngredientCommandProcessor
{
    private readonly ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> _createIngredientHandler;
public IngredientCommandProcessor(ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> createIngredientHandler)
    {
        _createIngredientHandler = createIngredientHandler;
    }

    public object? CreateIngredient(CreateIngredientQuery command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Command cannot be null");
        }

        if (string.IsNullOrWhiteSpace(command.name) || command.quantity <= 0 || command.restock_threshold < 0)
        {
            throw new ArgumentException("Invalid ingredient data provided");
        }

        return _createIngredientHandler.Handle(command);
    }
}