using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.CreateSale;

public class CreateSaleHandler : ICommandHandler<CreateSaleCommand, CreateSaleOutput>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;
    
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
    }

    public CreateSaleOutput Handle(CreateSaleCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Création de la vente
        var sale = new Sale
        {
            TableNumber = command.TableNumber,
            SaleDate = DateTime.Now,
            Status = "Pending", 
            OrderTimer = 15
        };

        // Ajout des items
        foreach (var itemDto in command.Items)
        {
            // Vérification que le mocktail existe
            var mocktail = _mocktailRepository.GetMocktailById(itemDto.MocktailId);
            if (mocktail == null)
                throw new Exception($"Mocktail {itemDto.MocktailId} not found");

            var saleItem = new SaleItem
            {
                MocktailId = itemDto.MocktailId,
                Quantity = itemDto.Quantity,
                ItemTotal = itemDto.Quantity * itemDto.UnitPrice
            };

            sale.SaleItems.Add(saleItem);
            sale.TotalAmount += saleItem.ItemTotal;
        }

        // Enregistrement
        var createdSale = _saleRepository.CreateSale(sale);

        return new CreateSaleOutput
        {
            Id = createdSale.Id,
            TotalAmount = createdSale.TotalAmount
        };
    }
}
        