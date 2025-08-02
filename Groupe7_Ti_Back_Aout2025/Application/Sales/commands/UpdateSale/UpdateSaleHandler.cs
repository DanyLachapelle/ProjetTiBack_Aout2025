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

        // 1. Charger la vente AVEC les items et les mocktails associés
        var sale = _saleRepository.GetSaleByIdWithItemsAndMocktails(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        // 2. Mise à jour du numéro de table
        if (!string.IsNullOrWhiteSpace(command.TableNumber))
            sale.TableNumber = command.TableNumber;

        // 2. Mise à jour du statut de la vente
        if (!string.IsNullOrWhiteSpace(command.status))
            sale.status = command.status;
        
        // 3. Mise à jour des items
        if (command.UpdatedItems != null && command.UpdatedItems.Any())
        {
            foreach (var itemDto in command.UpdatedItems)
            {
                var item = sale.SaleItems.FirstOrDefault(i => i.Id == itemDto.ItemId);
                if (item == null) continue;

                // a. Vérification et mise à jour du mocktail
                if (itemDto.NewMocktailId.HasValue)
                {
                    var newMocktail = _mocktailRepository.GetMocktailById(itemDto.NewMocktailId.Value);
                    if (newMocktail == null)
                        throw new Exception($"Mocktail {itemDto.NewMocktailId} not found");
                    
                    item.MocktailId = newMocktail.id;
                    item.Mocktail = newMocktail; // Mise à jour de la référence
                }

                // b. Mise à jour quantité
                if (itemDto.NewQuantity.HasValue)
                    item.Quantity = itemDto.NewQuantity.Value;
            }

            // 4. Recalcul du total BASÉ SUR LE PRIX DU MOCKTAIL
            RecalculateTotal(sale);
        }

        // 5. Sauvegarde
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
            // Récupération du prix via le mocktail associé
            if (item.Mocktail == null)
            {
                item.Mocktail = _mocktailRepository.GetMocktailById(item.MocktailId);
                if (item.Mocktail == null)
                    throw new Exception($"Mocktail {item.MocktailId} not found for item {item.Id}");
            }

            item.ItemTotal = item.Quantity * item.Mocktail.price;
            total += item.ItemTotal;
        }

        sale.TotalAmount = total;
    }
}