using System;
using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.DecreaseIngredientQuantity;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Ingredient.commands.UpdateQuantityIngredient;
using Application.Utils;

namespace Application.Ingredient.commands;

// Processeur central pour les commandes liées aux ingrédients
public class IngredientCommandProcessor
{
    // Handlers injectés pour chaque type de commande
    private readonly ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> _createIngredientHandler;
    private readonly ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput> _deleteIngredientHandler;
    private readonly ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput> _updateLimitIngredientHandler;
    private readonly ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput> _updateQuantityIngredientHandler;
    private readonly ICommandHandler<CreateIngredientCommand, CreateIngredientOutput> _createIngredientQueryHandler;
    private readonly ICommandHandler<DecreaseIngredientQuantityCommand, DecreaseIngredientQuantityOutput> _decreaseIngredientQuantityHandler;
    
    
    // Injection des dépendances
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
    
    // Suppression d'un ingrédient
    public DeleteIngredientOutput DeleteIngredient(int id)
    {
        var command = new DeleteIngredientCommand { Id = id };

        if (id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID provided");
        }

        return _deleteIngredientHandler.Handle(command);
    }
    
    // Mise à jour du seuil de réapprovisionnement
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
    
    // Augmentation de la quantité d'ingrédient
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
    
    // Création d'un nouvel ingrédient
    public CreateIngredientOutput CreateIngredient(CreateIngredientCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command), "Query cannot be null");
        }

        if (string.IsNullOrWhiteSpace(command.Name) || command.Quantity <= 0 || command.RestockThreshold < 0)
        {
            throw new ArgumentException("Invalid ingredient data provided");
        }

        return _createIngredientHandler.Handle(command);
    }
    
    // Diminution de la quantité d'ingrédient
    public DecreaseIngredientQuantityOutput DecreaseIngredientQuantity(int id, DecreaseIngredientQuantityCommand command)
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
            Quantity = command.Quantity
        };
        if (command.Quantity <= 0 || command.Id <= 0)
        {
            throw new ArgumentException("Invalid ingredient ID or quantity provided");
        }

        return _decreaseIngredientQuantityHandler.Handle(command);
    }
}