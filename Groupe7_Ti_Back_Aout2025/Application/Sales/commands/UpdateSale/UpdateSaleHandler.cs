using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.UpdateSale;

public class UpdateSaleHandler : ICommandHandler<UpdateSaleCommand, UpdateSaleOutput>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
    }

    public UpdateSaleOutput Handle(UpdateSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // 1. Load the sale WITH items and associated mocktails
        var sale = _saleRepository.GetSaleByIdWithItemsAndMocktails(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        // 2. Update table number
        if (!string.IsNullOrWhiteSpace(command.TableNumber))
            sale.TableNumber = command.TableNumber;

        // 2. Update sale status
        if (!string.IsNullOrWhiteSpace(command.Status))
            sale.Status = command.Status;
        
        // 3. Update items
        if (command.UpdatedItems != null && command.UpdatedItems.Any())
        {
            foreach (var itemDto in command.UpdatedItems)
            {
                var item = sale.SaleItems.FirstOrDefault(i => i.Id == itemDto.ItemId);
                if (item == null) continue;

                // a. Verify and update mocktail
                if (itemDto.NewMocktailId.HasValue)
                {
                    var newMocktail = _mocktailRepository.GetMocktailById(itemDto.NewMocktailId.Value);
                    if (newMocktail == null)
                        throw new Exception($"Mocktail {itemDto.NewMocktailId} not found");
                    
                    item.MocktailId = newMocktail.Id;
                    item.Mocktail = newMocktail; // Update reference
                }

                // b. Update quantity
                if (itemDto.NewQuantity.HasValue)
                    item.Quantity = itemDto.NewQuantity.Value;
            }

            // 4. Recalculate total BASED ON MOCKTAIL PRICE
            RecalculateTotal(sale);
        }

        // 5. Save changes
        _saleRepository.UpdateSale(sale);

        return new UpdateSaleOutput
        {
            UpdatedId = sale.Id,
            NewTableNumber = sale.TableNumber,
            NewTotalAmount = sale.TotalAmount
        };
    }

    private void RecalculateTotal(Sale sale)
    {
        decimal total = 0;
        
        foreach (var item in sale.SaleItems)
        {
            // Get price via associated mocktail
            if (item.Mocktail == null)
            {
                item.Mocktail = _mocktailRepository.GetMocktailById(item.MocktailId);
                if (item.Mocktail == null)
                    throw new Exception($"Mocktail {item.MocktailId} not found for item {item.Id}");
            }

            item.ItemTotal = item.Quantity * item.Mocktail.Price;
            total += item.ItemTotal;
        }

        sale.TotalAmount = total;
    }
}