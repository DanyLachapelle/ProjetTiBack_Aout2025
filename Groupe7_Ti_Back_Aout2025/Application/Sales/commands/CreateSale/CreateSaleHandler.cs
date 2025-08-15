using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.CreateSale;

// Handler for creating new sales with mocktail items
public class CreateSaleHandler : ICommandHandler<CreateSaleCommand, CreateSaleOutput>
{
    // Dependencies for sales and mocktails data access
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;
    
    // Constructor with dependency injection
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
    }

    // Main handler method
    public CreateSaleOutput Handle(CreateSaleCommand command)
    {
        // Input validation
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Initialize new sale with basic information
        var sale = new Sale
        {
            TableNumber = command.TableNumber,
            SaleDate = DateTime.Now,  // Sets current timestamp
            Status = "Pending",       // Initial state
            OrderTimer = 15           // Default preparation time
        };

        // Process each item in the order
        foreach (var itemDto in command.Items)
        {
            // Verify mocktail exists
            var mocktail = _mocktailRepository.GetMocktailById(itemDto.MocktailId);
            if (mocktail == null)
                throw new Exception($"Mocktail {itemDto.MocktailId} not found");

            // Create sale item with calculated total
            var saleItem = new SaleItem
            {
                MocktailId = itemDto.MocktailId,
                Quantity = itemDto.Quantity,
                ItemTotal = itemDto.Quantity * itemDto.UnitPrice  // Calculate line total
            };

            // Add item to sale
            sale.SaleItems.Add(saleItem);
            sale.TotalAmount += saleItem.ItemTotal;  // Accumulate order total
        }

        // Persist the sale
        var createdSale = _saleRepository.CreateSale(sale);

        // Return creation result
        return new CreateSaleOutput
        {
            Id = createdSale.Id,
            TotalAmount = createdSale.TotalAmount
        };
    }
}