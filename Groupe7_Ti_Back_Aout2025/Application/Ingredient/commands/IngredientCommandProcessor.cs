using System;
using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.DecreaseIngredientQuantity;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Ingredient.commands.UpdateQuantityIngredient;
using Application.Utils;

namespace Application.Ingredient.commands;

public class IngredientCommandProcessor
{
    private readonly ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> _createIngredientHandler;
    private readonly ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput> _deleteIngredientHandler;
    private readonly ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> _updateLimitIngredientHandler;
    private readonly ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput> _updateQuantityIngredientHandler;
    private readonly ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> _createIngredientQueryHandler;
    private readonly ICommandHandler<DecreaseIngredientQuantityCommand, DecreaseIngredientQuantityOutput> _decreaseIngredientQuantityHandler;
    
    
    public IngredientCommandProcessor(
        ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> createIngredientHandler,
        ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput> deleteIngredientHandler,
        ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> updateLimitIngredientHandler,
        ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput> updateQuantityIngredientHandler,
        ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> createIngredientQueryHandler,
        ICommandHandler<DecreaseIngredientQuantityCommand, DecreaseIngredientQuantityOutput> decreaseIngredientQuantityHandler)
    {
        _createIngredientHandler = createIngredientHandler;
        _deleteIngredientHandler = deleteIngredientHandler;
        _updateLimitIngredientHandler = updateLimitIngredientHandler;
        _updateQuantityIngredientHandler = updateQuantityIngredientHandler;
        _createIngredientQueryHandler = createIngredientQueryHandler;
        _decreaseIngredientQuantityHandler = decreaseIngredientQuantityHandler;
    }
    
    public DeleteIngredientOutput DeleteIngredient(int id)
    {
        var command = new DeleteIngredientCommand { id = id };

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
    
    public UpdateQuantityIngredientOutput UpdateQuantityIngredient(int id, UpdateQuantityIngredientCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (id <= 0)
            throw new ArgumentException("Invalid ingredient ID");

        var commands = new UpdateQuantityIngredientCommand
        {
            Id = id,
            Amount = command.Amount
        };

        return _updateQuantityIngredientHandler.Handle(command);
    }
    
    public CreateIngredientOutput CreateIngredient(CreateIngredientCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Query cannot be null");
        }

        if (string.IsNullOrWhiteSpace(command.name) || command.quantity <= 0 || command.restock_threshold < 0)
        {
            throw new ArgumentException("Invalid ingredient data provided");
        }

        // Valider l'unité
        if (!Domain.Ingredient.ValidUnits.Contains(command.unit))
        {
            throw new ArgumentException($"Invalid unit '{command.unit}'. Valid units are: {string.Join(", ", Domain.Ingredient.ValidUnits)}");
        }

        // Nettoyer et valider l'allergène
        var allergen = string.IsNullOrWhiteSpace(command.allergen) ? "none" : command.allergen.ToLower().Trim();
        if (!Domain.Ingredient.ValidAllergens.Contains(allergen))
        {
            throw new ArgumentException($"Invalid allergen '{command.allergen}'. Valid allergens are: {string.Join(", ", Domain.Ingredient.ValidAllergens)}");
        }

        return _createIngredientHandler.Handle(command);
    }
    
    public DecreaseIngredientQuantityOutput DecreaseIngredientQuantity(int id,DecreaseIngredientQuantityCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Command cannot be null");
        }

        if (id <= 0)
            throw new ArgumentException("Invalid ingredient ID");

        var commands = new DecreaseIngredientQuantityCommand()
        {
            Id = id,
            quantity = command.quantity
        };
        if (command.quantity <= 0 || command.Id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID or quantity provided");
        }

        return _decreaseIngredientQuantityHandler.Handle(command);
    }



    
}