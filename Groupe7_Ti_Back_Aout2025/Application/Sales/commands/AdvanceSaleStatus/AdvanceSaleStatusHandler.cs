using Application.Utils;
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
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Get the sale
        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale with ID {command.SaleId} not found");

        // Define status progression
        var statusProgression = new Dictionary<string, string>
        {
            { "Pending", "IN_PREPARATION" },
            { "PENDING", "IN_PREPARATION" },
            { "IN_PREPARATION", "READY" },
            { "READY", "DELIVERED" },
            { "DELIVERED", "DELIVERED" } // Can't advance beyond delivered
        };

        var currentStatus = sale.Status ?? "Pending";
        
        if (!statusProgression.ContainsKey(currentStatus))
        {
            throw new Exception($"Unknown status: {currentStatus}");
        }

        var newStatus = statusProgression[currentStatus];
        
        // Don't update if already at final status
        if (currentStatus == "DELIVERED")
        {
            return new AdvanceSaleStatusOutput
            {
                saleId = command.SaleId,
                NewStatus = currentStatus,
                Message = "Order is already delivered"
            };
        }

        // Update the status
        sale.Status = newStatus;
        _saleRepository.UpdateSale(sale);

        return new AdvanceSaleStatusOutput
        {
            saleId = command.SaleId,
            NewStatus = newStatus,
            Message = $"Status updated from {currentStatus} to {newStatus}"
        };
    }
}
