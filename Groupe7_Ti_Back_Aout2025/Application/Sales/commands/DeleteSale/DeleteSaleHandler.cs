using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.DeleteSale;

public class DeleteSaleHandler : ICommandHandler<DeleteSaleCommand, DeleteSaleOutput>
{
    private readonly ISaleRepository _saleRepository;
    
    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public DeleteSaleOutput Handle(DeleteSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        _saleRepository.DeleteSale(sale);

        return new DeleteSaleOutput 
        {
            Success = true,
            DeletedId = command.SaleId
        };
    }
}