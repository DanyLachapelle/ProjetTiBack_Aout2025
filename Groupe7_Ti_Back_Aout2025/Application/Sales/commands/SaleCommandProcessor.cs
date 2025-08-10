using Application.Sales.commands.CreateSale;
using Application.Sales.commands.DeleteSale;
using Application.Sales.commands.UpdateSale;
using Application.Sales.commands.AdvanceSaleStatus;
using Application.Utils;

namespace Application.Sales.commands;

public class SaleCommandProcessor
{
    private readonly ICommandHandler<CreateSaleCommand, CreateSaleOutput> _createSaleHandler;
    private readonly ICommandHandler<UpdateSaleCommand, UpdateSaleOutput> _updateSaleHandler;
    private readonly ICommandHandler<DeleteSaleCommand, DeleteSaleOutput> _deleteSaleHandler;
    private readonly ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput> _advanceSaleStatusHandler;

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
    public CreateSaleOutput CreateSale(CreateSaleCommand command)
    {
        return _createSaleHandler.Handle(command);
    }
    
    public UpdateSaleOutput UpdateSale(UpdateSaleCommand command)
    {
        return _updateSaleHandler.Handle(command);
    }
    
    public DeleteSaleOutput DeleteSale(DeleteSaleCommand command)
    {
        return _deleteSaleHandler.Handle(command);
    }
    
    public AdvanceSaleStatusOutput AdvanceSaleStatus(AdvanceSaleStatusCommand command)
    {
        return _advanceSaleStatusHandler.Handle(command);
    }
    
}