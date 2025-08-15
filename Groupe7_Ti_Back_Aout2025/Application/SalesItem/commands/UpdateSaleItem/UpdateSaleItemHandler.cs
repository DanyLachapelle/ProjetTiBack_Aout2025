using Application.Utils;
using Infrastructure.Mocktail;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.commands.UpdateSaleItem;

public class UpdateSaleItemHandler : ICommandHandler<UpdateSaleItemCommand, UpdateSaleItemOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;

    public UpdateSaleItemHandler(
        ISaleItemRepository saleItemRepository,
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository)
    {
        _saleItemRepository = saleItemRepository;
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
    }

    public UpdateSaleItemOutput Handle(UpdateSaleItemCommand command)
    {
        // Validation
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (command.NewQuantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(command.NewQuantity));

        // Retrieve item with its relations
        var item = _saleItemRepository.GetByIdWithItems(command.ItemId);
        if (item == null || item.SaleId != command.SaleId)
            throw new KeyNotFoundException($"Item {command.ItemId} not found in sale {command.SaleId}");

        // Retrieve mocktail for price
        var mocktail = _mocktailRepository.GetMocktailById(item.MocktailId);
        var unitPrice = mocktail?.Price ?? 0;

        // Update
        item.Quantity = command.NewQuantity;
        item.ItemTotal = command.NewQuantity * unitPrice;

        // Save
        _saleItemRepository.Update(item);

        // Update sale total
        var sale = _saleRepository.GetSaleById(command.SaleId);
        sale.TotalAmount = _saleItemRepository.GetBySaleId(command.SaleId)
            .Sum(i => i.ItemTotal);
        _saleRepository.UpdateSale(_saleRepository.GetSaleById(command.SaleId));

        return new UpdateSaleItemOutput(
            UpdatedItemId: item.Id,
            NewQuantity: item.Quantity,
            NewItemTotal: item.ItemTotal,
            NewSaleTotal: sale.TotalAmount
        );
    }
}