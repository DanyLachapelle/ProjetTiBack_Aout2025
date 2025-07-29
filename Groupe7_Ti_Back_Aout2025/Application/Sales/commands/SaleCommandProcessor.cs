using Application.Sales.commands.CreateSale;
using Application.Utils;

namespace Application.Sales.commands;

public class SaleCommandProcessor
{
    private readonly ICommandHandler<CreateSaleCommand, CreateSaleOutput> _createSaleHandler;
    
    public SaleCommandProcessor(ICommandHandler<CreateSaleCommand, CreateSaleOutput> createSaleHandler)
    {
        _createSaleHandler = createSaleHandler;
    }
    
    public CreateSaleOutput CreateSale(CreateSaleCommand command)
    {
        return _createSaleHandler.Handle(command);
    }
    
}