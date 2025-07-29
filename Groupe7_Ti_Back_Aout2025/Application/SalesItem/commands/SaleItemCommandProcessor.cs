using Application.SalesItem.commands.AddItemToSale;
using Application.SalesItem.commands.RemoveItemFromSale;
using Application.SalesItem.commands.UpdateSaleItem;
using Application.Utils;

namespace Application.SalesItem.commands;

public class SaleItemCommandProcessor
{
    private readonly ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput> _addItemToSaleHandler;
    private readonly ICommandHandler<RemoveItemFromSaleCommand, RemoveItemFromSaleOutput> _removeItemFromSaleHandler;
    private readonly ICommandHandler<UpdateSaleItemCommand, UpdateSaleItemOutput> _updateSaleItemHandler;
    
    public SaleItemCommandProcessor(
        ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput> addItemToSaleHandler,
        ICommandHandler<RemoveItemFromSaleCommand, RemoveItemFromSaleOutput> removeItemFromSaleHandler,
        ICommandHandler<UpdateSaleItemCommand, UpdateSaleItemOutput> updateSaleItemHandler)
    {
        _addItemToSaleHandler = addItemToSaleHandler;
        _removeItemFromSaleHandler = removeItemFromSaleHandler;
        _updateSaleItemHandler = updateSaleItemHandler;
    }
    
    public AddItemToSaleOutput AddItemToSale(AddItemToSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _addItemToSaleHandler.Handle(command);
    }
    
    public RemoveItemFromSaleOutput RemoveItemFromSale(RemoveItemFromSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _removeItemFromSaleHandler.Handle(command);
    }
    
    public UpdateSaleItemOutput UpdateSaleItem(UpdateSaleItemCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return _updateSaleItemHandler.Handle(command);
    }
    
    
}