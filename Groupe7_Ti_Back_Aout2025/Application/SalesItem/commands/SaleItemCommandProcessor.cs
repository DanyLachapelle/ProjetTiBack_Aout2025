using Application.SalesItem.commands.AddItemToSale;
using Application.SalesItem.commands.RemoveItemFromSale;
using Application.SalesItem.commands.UpdateSaleItem;
using Application.Utils;

namespace Application.SalesItem.commands;

// Handles routing of sale item commands to their respective handlers
public class SaleItemCommandProcessor
{
    private readonly ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput> _addItemToSaleHandler;
    private readonly ICommandHandler<RemoveItemFromSaleCommand, RemoveItemFromSaleOutput> _removeItemFromSaleHandler;
    private readonly ICommandHandler<UpdateSaleItemCommand, UpdateSaleItemOutput> _updateSaleItemHandler;
    
    // Initializes command handlers through dependency injection
    public SaleItemCommandProcessor(
        ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput> addItemToSaleHandler,
        ICommandHandler<RemoveItemFromSaleCommand, RemoveItemFromSaleOutput> removeItemFromSaleHandler,
        ICommandHandler<UpdateSaleItemCommand, UpdateSaleItemOutput> updateSaleItemHandler)
    {
        _addItemToSaleHandler = addItemToSaleHandler;
        _removeItemFromSaleHandler = removeItemFromSaleHandler;
        _updateSaleItemHandler = updateSaleItemHandler;
    }
    
    // Processes item addition to a sale
    public AddItemToSaleOutput AddItemToSale(AddItemToSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _addItemToSaleHandler.Handle(command);
    }
    
    // Processes item removal from a sale
    public RemoveItemFromSaleOutput RemoveItemFromSale(RemoveItemFromSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _removeItemFromSaleHandler.Handle(command);
    }
    
    // Processes item quantity/price updates in a sale
    public UpdateSaleItemOutput UpdateSaleItem(UpdateSaleItemCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _updateSaleItemHandler.Handle(command);
    }
}