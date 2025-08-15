using Application.Sales.commands.AdvanceSaleStatus;
using Application.Sales.commands.CreateSale;
using Application.Sales.commands.DeleteSale;
using Application.Sales.commands.UpdateSale;
using Application.Utils;

namespace Application.Sales.commands;

// Central facade for sales-related commands
public class SaleCommandProcessor
{
    // Handlers for each command type
    private readonly ICommandHandler<CreateSaleCommand, CreateSaleOutput> _createSaleHandler;
    private readonly ICommandHandler<UpdateSaleCommand, UpdateSaleOutput> _updateSaleHandler;
    private readonly ICommandHandler<DeleteSaleCommand, DeleteSaleOutput> _deleteSaleHandler;
    private readonly ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput> _advanceSaleStatusHandler;

    // Dependency injection
    public SaleCommandProcessor(
        ICommandHandler<CreateSaleCommand, CreateSaleOutput> createSaleHandler,
        ICommandHandler<UpdateSaleCommand, UpdateSaleOutput> updateSaleHandler,
        ICommandHandler<DeleteSaleCommand, DeleteSaleOutput> deleteSaleHandler,
        ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput> advanceSaleStatusHandler)
    {
        _createSaleHandler = createSaleHandler;
        _updateSaleHandler = updateSaleHandler;
        _deleteSaleHandler = deleteSaleHandler;
        _advanceSaleStatusHandler = advanceSaleStatusHandler;
    }

    // Creates a new sale
    public CreateSaleOutput CreateSale(CreateSaleCommand command)
    {
        return _createSaleHandler.Handle(command);
    }
    
    // Updates an existing sale
    public UpdateSaleOutput UpdateSale(UpdateSaleCommand command)
    {
        return _updateSaleHandler.Handle(command);
    }
    
    // Deletes a sale
    public DeleteSaleOutput DeleteSale(DeleteSaleCommand command)
    {
        return _deleteSaleHandler.Handle(command);
    }
    
    // Advances a sale's status
    public AdvanceSaleStatusOutput AdvanceSaleStatus(AdvanceSaleStatusCommand command)
    {
        return _advanceSaleStatusHandler.Handle(command);
    }
}