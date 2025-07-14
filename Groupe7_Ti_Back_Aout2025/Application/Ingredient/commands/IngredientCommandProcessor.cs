using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Ingredient.commands.UpdateQuantityIngredient;
using Application.Utils;

namespace Application.Ingredient.commands;

public class IngredientCommandProcessor
{
    private readonly ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> _createIngredientHandler;
    private readonly ICommandHandler<DeleteIngredientQuery, DeleteIngredientOutput> _deleteIngredientHandler;
    private readonly ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> _updateLimitIngredientHandler;
    private readonly ICommandHandler<UpdateQuantityIngredientQuery, UpdateQuantityIngredientOutput> _updateQuantityIngredientHandler;
    
    public IngredientCommandProcessor(
        ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> createIngredientHandler,
        ICommandHandler<DeleteIngredientQuery, DeleteIngredientOutput> deleteIngredientHandler,
        ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> updateLimitIngredientHandler,
        ICommandHandler<UpdateQuantityIngredientQuery, UpdateQuantityIngredientOutput> updateQuantityIngredientHandler)
    {
        _createIngredientHandler = createIngredientHandler;
        _deleteIngredientHandler = deleteIngredientHandler;
        _updateLimitIngredientHandler = updateLimitIngredientHandler;
        _updateQuantityIngredientHandler = updateQuantityIngredientHandler;
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
    
    public UpdateQuantityIngredientOutput UpdateQuantityIngredient(int id, UpdateQuantityIngredientQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (id <= 0)
            throw new ArgumentException("Invalid ingredient ID");

        var command = new UpdateQuantityIngredientQuery
        {
            Id = id,
            Amount = query.Amount
        };

        return _updateQuantityIngredientHandler.Handle(command);
    }



    
}