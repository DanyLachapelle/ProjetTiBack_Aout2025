using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.DeleteSale;

// Handler for deleting a sale
public class DeleteSaleHandler : ICommandHandler<DeleteSaleCommand, DeleteSaleOutput>
{
    // Repository for sales data access
    private readonly ISaleRepository _saleRepository;
    
    // Repository dependency injection
    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    // Main command processing method
    public DeleteSaleOutput Handle(DeleteSaleCommand command)
    {
        // Command validation
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Retrieving the sale
        var sale = _saleRepository.GetSaleById(command.SaleId);
        
        // Checking if sale exists
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        // Actual deletion
        _saleRepository.DeleteSale(sale);

        // Returning the result
        return new DeleteSaleOutput 
        {
            Success = true,
            DeletedId = command.SaleId
        };
    }
}