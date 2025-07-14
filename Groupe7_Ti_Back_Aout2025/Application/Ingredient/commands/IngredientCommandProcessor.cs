using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Utils;

namespace Application.Ingredient.commands;

public class IngredientCommandProcessor
{
    private readonly ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> _createIngredientHandler;
    private readonly ICommandHandler<DeleteIngredientQuery, DeleteIngredientOutput> _deleteIngredientHandler;
    private readonly ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> _updateLimitIngredientHandler;
    
    public IngredientCommandProcessor(
        ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> createIngredientHandler,
        ICommandHandler<DeleteIngredientQuery, DeleteIngredientOutput> deleteIngredientHandler,
        ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> updateLimitIngredientHandler)
    {
        _createIngredientHandler = createIngredientHandler ?? throw new ArgumentNullException(nameof(createIngredientHandler));
        _deleteIngredientHandler = deleteIngredientHandler ?? throw new ArgumentNullException(nameof(deleteIngredientHandler));
        _updateLimitIngredientHandler = updateLimitIngredientHandler ?? throw new ArgumentNullException(nameof(updateLimitIngredientHandler));
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
    
    public DeleteIngredientOutput DeleteIngredient(int id)
    {
        var command = new DeleteIngredientQuery { id = id };

        if (id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID provided");
        }

        return _deleteIngredientHandler.Handle(command);
    }
    
    public UpdateLimitIngredientOutput UpdateLimitIngredient(int id, UpdateLimitIngredientQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (id <= 0)
            throw new ArgumentException("Invalid ingredient ID");

        var command = new UpdateLimitIngredientCommand
        {
            Id = id,
            RestockThreshold = query.RestockThreshold
        };

        return _updateLimitIngredientHandler.Handle(command);
    }



    
}