using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.DeleteSale;

// Handler pour la suppression d'une vente
public class DeleteSaleHandler : ICommandHandler<DeleteSaleCommand, DeleteSaleOutput>
{
    // Répository pour l'accès aux données des ventes
    private readonly ISaleRepository _saleRepository;
    
    // Injection de dépendance du repository
    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    // Méthode principale de traitement de la commande
    public DeleteSaleOutput Handle(DeleteSaleCommand command)
    {
        // Validation de la commande
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Récupération de la vente
        var sale = _saleRepository.GetSaleById(command.SaleId);
        
        // Vérification de l'existence de la vente
        if (sale == null)
            throw new Exception($"Sale {command.SaleId} not found");

        // Suppression effective
        _saleRepository.DeleteSale(sale);

        // Retour du résultat
        return new DeleteSaleOutput 
        {
            Success = true,
            DeletedId = command.SaleId
        };
    }
}