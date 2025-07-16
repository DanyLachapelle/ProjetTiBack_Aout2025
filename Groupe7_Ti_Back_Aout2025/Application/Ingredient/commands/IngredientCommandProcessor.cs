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
    private readonly ICommandHandler<CreateIngredientQuery, CreateIngredientOutput> _createIngredientQueryHandler;
    
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
    
    public CreateIngredientOutput CreateIngredient(CreateIngredientQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query), "Query cannot be null");
        }

        if (string.IsNullOrWhiteSpace(query.name) || query.quantity <= 0 || query.restock_threshold < 0)
        {
            throw new ArgumentException("Invalid ingredient data provided");
        }

        return _createIngredientHandler.Handle(query);
    }



    
}