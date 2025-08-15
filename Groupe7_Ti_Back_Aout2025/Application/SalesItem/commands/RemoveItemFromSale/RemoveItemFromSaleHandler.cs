using Application.Utils;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.commands.RemoveItemFromSale;

public class RemoveItemFromSaleHandler : ICommandHandler<RemoveItemFromSaleCommand, RemoveItemFromSaleOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;

    public RemoveItemFromSaleHandler(
        ISaleItemRepository saleItemRepository,
        ISaleRepository saleRepository)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
    }

    public RemoveItemFromSaleOutput Handle(RemoveItemFromSaleCommand command)
    {
        // Input validation
        if (command == null)
            throw new ArgumentNullException(nameof(command), "Command cannot be null");

        if (command.SaleId <= 0)
            throw new ArgumentException("Invalid sale ID provided", nameof(command.SaleId));

        if (command.ItemId <= 0)
            throw new ArgumentException("Invalid item ID provided", nameof(command.ItemId));

        // Retrieve item with ownership check
        var item = _saleItemRepository.GetById(command.ItemId);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {command.ItemId} not found");

        if (item.SaleId != command.SaleId)
            throw new InvalidOperationException("Item does not belong to this sale");

        // Delete operation
        _saleItemRepository.Remove(item);
        
        // Update sale total
        var sale = _saleItemRepository.GetById(command.SaleId);
        sale.TotalAmount = _saleItemRepository.GetBySaleId(command.SaleId)
            .Sum(i => i.ItemTotal);
        _saleRepository.UpdateSale(_saleRepository.GetSaleById(command.SaleId));

        return new RemoveItemFromSaleOutput
        {
            Success = true,
            RemovedItemId = item.Id,
        };
    }
}