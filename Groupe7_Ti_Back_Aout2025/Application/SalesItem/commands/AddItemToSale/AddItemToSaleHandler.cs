using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.commands.AddItemToSale;

// Handler for adding an item to an existing sale
public class AddItemToSaleHandler : ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput>
{
    // Required repositories
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    
    // Dependency injection
    public AddItemToSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository,
        ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
        _saleItemRepository = saleItemRepository;
    }

    // Main processing method
    public AddItemToSaleOutput Handle(AddItemToSaleCommand command)
    {
        // ========== 1. VALIDATION ========== //
        
        // Verify sale exists
        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Vente {command.SaleId} introuvable");

        // Verify mocktail exists
        var mocktail = _mocktailRepository.GetMocktailById(command.MocktailId);
        if (mocktail == null)
            throw new Exception($"Mocktail {command.MocktailId} introuvable");

        // Validate quantity
        if (command.Quantity <= 0)
            throw new Exception("La quantité doit être positive");

        // ========== 2. ITEM CREATION ========== //
        
        var item = new SaleItem
        {
            SaleId = command.SaleId,
            MocktailId = command.MocktailId,
            Quantity = command.Quantity,
            ItemTotal = command.Quantity * mocktail.Price // Calculate subtotal
        };

        // ========== 3. PERSISTENCE ========== //
        
        // Add item to sale
        _saleRepository.AddSaleItem(item);

        // ========== 4. TOTAL UPDATE ========== //
        
        // Get all sale items
        var allItems = _saleItemRepository.GetBySaleId(command.SaleId);
        
        // Calculate new total
        sale.TotalAmount = allItems.Sum(i => i.ItemTotal);
        
        // Update sale
        _saleRepository.UpdateSale(sale);

        // ========== 5. RESULT ========== //
        
        return new AddItemToSaleOutput(
            ItemId: item.Id,
            NewTotalAmount: sale.TotalAmount
        );
    }
}