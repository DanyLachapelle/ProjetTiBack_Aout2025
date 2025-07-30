using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.commands.AddItemToSale;

public class AddItemToSaleHandler : ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    
    public AddItemToSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository,
        ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
        _saleItemRepository = saleItemRepository;
    }

    public AddItemToSaleOutput Handle(AddItemToSaleCommand command)
    {
        // 1. Validation
        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        var mocktail = _mocktailRepository.GetMocktailById(command.MocktailId);
        if (mocktail == null)
            throw new Exception($"Mocktail {command.MocktailId} not found");

        if (command.Quantity <= 0)
            throw new Exception("Quantity must be positive");

        // 2. Création de l'item
        var item = new SaleItem
        {
            SaleId = command.SaleId,
            MocktailId = command.MocktailId,
            Quantity = command.Quantity,
            ItemTotal = command.Quantity * mocktail.price
        };

        // 3. Sauvegarde de l'item
        _saleRepository.AddSaleItem(item);

        // 4. ➕ Recalcul et mise à jour du total de la vente
        var allItems = _saleItemRepository.GetBySaleId(command.SaleId);
        sale.TotalAmount = allItems.Sum(i => i.ItemTotal);
        _saleRepository.UpdateSale(sale);

        return new AddItemToSaleOutput(
            ItemId: item.Id,
            NewTotalAmount: sale.TotalAmount
        );
    }

}