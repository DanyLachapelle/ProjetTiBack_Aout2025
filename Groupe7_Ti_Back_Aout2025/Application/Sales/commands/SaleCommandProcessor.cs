using Application.Sales.commands.AdvanceSaleStatus;
using Application.Sales.commands.CreateSale;
using Application.Sales.commands.DeleteSale;
using Application.Sales.commands.UpdateSale;
using Application.Utils;

namespace Application.Sales.commands;

// Façade centrale pour les commandes relatives aux ventes
public class SaleCommandProcessor
{
    // Handlers pour chaque type de commande
    private readonly ICommandHandler<CreateSaleCommand, CreateSaleOutput> _createSaleHandler;
    private readonly ICommandHandler<UpdateSaleCommand, UpdateSaleOutput> _updateSaleHandler;
    private readonly ICommandHandler<DeleteSaleCommand, DeleteSaleOutput> _deleteSaleHandler;
    private readonly ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput> _advanceSaleStatusHandler;

    // Injection des dépendances
    public SaleCommandProcessor(
        ICommandHandler<CreateSaleCommand, CreateSaleOutput> createSaleHandler,
        ICommandHandler<UpdateSaleCommand, UpdateSaleOutput> updateSaleHandler,
        ICommandHandler<DeleteSaleCommand, DeleteSaleOutput> deleteSaleHandler,
        ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput> advanceSaleStatusHandler)
    {
        _createSaleHandler = createSaleHandler;
        _updateSaleHandler = updateSaleHandler;
        _deleteSaleHandler = deleteSaleHandler;
        _advanceSaleStatusHandler = advanceSaleStatusHandler;
    }

    // Crée une nouvelle vente
    public CreateSaleOutput CreateSale(CreateSaleCommand command)
    {
        return _createSaleHandler.Handle(command);
    }
    
    // Met à jour une vente existante
    public UpdateSaleOutput UpdateSale(UpdateSaleCommand command)
    {
        return _updateSaleHandler.Handle(command);
    }
    
    // Supprime une vente
    public DeleteSaleOutput DeleteSale(DeleteSaleCommand command)
    {
        return _deleteSaleHandler.Handle(command);
    }
    
    // Fait avancer le statut d'une vente
    public AdvanceSaleStatusOutput AdvanceSaleStatus(AdvanceSaleStatusCommand command)
    {
        return _advanceSaleStatusHandler.Handle(command);
    }
}