using Application.Utils;
using Domain;
using Infrastructure.Mocktail;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.commands.AddItemToSale;

// Handler pour l'ajout d'un item à une vente existante
public class AddItemToSaleHandler : ICommandHandler<AddItemToSaleCommand, AddItemToSaleOutput>
{
    // Répositories nécessaires
    private readonly ISaleRepository _saleRepository;
    private readonly IMocktailRepository _mocktailRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    
    // Injection des dépendances
    public AddItemToSaleHandler(
        ISaleRepository saleRepository,
        IMocktailRepository mocktailRepository,
        ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _mocktailRepository = mocktailRepository;
        _saleItemRepository = saleItemRepository;
    }

    // Méthode principale de traitement
    public AddItemToSaleOutput Handle(AddItemToSaleCommand command)
    {
        // ========== 1. VALIDATION ========== //
        
        // Vérification de l'existence de la vente
        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Vente {command.SaleId} introuvable");

        // Vérification de l'existence du mocktail
        var mocktail = _mocktailRepository.GetMocktailById(command.MocktailId);
        if (mocktail == null)
            throw new Exception($"Mocktail {command.MocktailId} introuvable");

        // Validation de la quantité
        if (command.Quantity <= 0)
            throw new Exception("La quantité doit être positive");

        // ========== 2. CRÉATION DE L'ITEM ========== //
        
        var item = new SaleItem
        {
            SaleId = command.SaleId,
            MocktailId = command.MocktailId,
            Quantity = command.Quantity,
            ItemTotal = command.Quantity * mocktail.Price // Calcul du sous-total
        };

        // ========== 3. PERSISTANCE ========== //
        
        // Ajout de l'item à la vente
        _saleRepository.AddSaleItem(item);

        // ========== 4. MISE À JOUR DU TOTAL ========== //
        
        // Récupération de tous les items de la vente
        var allItems = _saleItemRepository.GetBySaleId(command.SaleId);
        
        // Calcul du nouveau total
        sale.TotalAmount = allItems.Sum(i => i.ItemTotal);
        
        // Mise à jour de la vente
        _saleRepository.UpdateSale(sale);

        // ========== 5. RÉSULTAT ========== //
        
        return new AddItemToSaleOutput(
            ItemId: item.Id,
            NewTotalAmount: sale.TotalAmount
        );
    }
}