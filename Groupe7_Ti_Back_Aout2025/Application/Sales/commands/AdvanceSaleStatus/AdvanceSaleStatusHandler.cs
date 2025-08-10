using Application.Utils;
using Domain;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.AdvanceSaleStatus;

public class AdvanceSaleStatusHandler : ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput>
{
    private readonly ISaleRepository _saleRepository;

    public AdvanceSaleStatusHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public AdvanceSaleStatusOutput Handle(AdvanceSaleStatusCommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null) throw new Exception($"Sale {command.SaleId} not found");

        sale.status = GetNextStatus(sale.status);

        _saleRepository.UpdateSale(sale);

        return new AdvanceSaleStatusOutput
        {
            SaleId = sale.Id,
            NewStatus = sale.status
        };
    }

    private static string GetNextStatus(string current)
    {
        // Normaliser au cas où
        var normalized = (current ?? string.Empty).Trim().ToUpperInvariant();

        return normalized switch
        {
            var s when s == SaleStatus.Pending => SaleStatus.InPreparation,
            var s when s == SaleStatus.InPreparation => SaleStatus.Ready,
            var s when s == SaleStatus.Ready => SaleStatus.Delivered,
            var s when s == SaleStatus.Delivered => SaleStatus.Delivered,
            _ => SaleStatus.Pending
        };
    }
}



