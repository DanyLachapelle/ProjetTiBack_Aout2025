using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.commands.AdvanceSaleStatus;

// Handler for advancing the status of a sale through its lifecycle
public class AdvanceSaleStatusHandler : ICommandHandler<AdvanceSaleStatusCommand, AdvanceSaleStatusOutput>
{
    private readonly ISaleRepository _saleRepository;

    // Dependency injection of sale repository
    public AdvanceSaleStatusHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public AdvanceSaleStatusOutput Handle(AdvanceSaleStatusCommand command)
    {
        // Input validation
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Retrieve sale entity
        var sale = _saleRepository.GetSaleById(command.SaleId);
        if (sale == null)
            throw new Exception($"Sale with ID {command.SaleId} not found");

        // Status transition mapping
        var statusProgression = new Dictionary<string, string>
        {
            { "Pending", "IN_PREPARATION" },  // Case-insensitive fallback
            { "PENDING", "IN_PREPARATION" },   // Standardized format
            { "IN_PREPARATION", "READY" },
            { "READY", "DELIVERED" },
            { "DELIVERED", "DELIVERED" }      // Terminal state
        };

        // Handle null status
        var currentStatus = sale.Status ?? "Pending";
        
        // Validate current status
        if (!statusProgression.ContainsKey(currentStatus))
        {
            throw new Exception($"Unknown status: {currentStatus}");
        }

        // Determine next status
        var newStatus = statusProgression[currentStatus];
        
        // Check for terminal state
        if (currentStatus == "DELIVERED")
        {
            return new AdvanceSaleStatusOutput
            {
                saleId = command.SaleId,
                NewStatus = currentStatus,
                Message = "Order is already delivered"
            };
        }

        // Update and persist status
        sale.Status = newStatus;
        _saleRepository.UpdateSale(sale);

        // Return transition result
        return new AdvanceSaleStatusOutput
        {
            saleId = command.SaleId,
            NewStatus = newStatus,
            Message = $"Status updated from {currentStatus} to {newStatus}"
        };
    }
}