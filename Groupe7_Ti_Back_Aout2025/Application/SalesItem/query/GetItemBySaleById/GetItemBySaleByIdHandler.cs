using Application.Utils;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.query.GetItemBySaleById;

// Handles retrieval of a specific sale item by ID within a sale context
public class GetItemBySaleByIdHandler : IQueryHandler<GetItemBySaleByIdQuery, GetItemBySaleByIdOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;

    // Initializes with sale item repository dependency
    public GetItemBySaleByIdHandler(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    // Retrieves and validates a sale item, returns null if not found or mismatched
    public GetItemBySaleByIdOutput Handle(GetItemBySaleByIdQuery query)
    {
        // Input validation
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        // Directly uses existing repository method
        var item = _saleItemRepository.GetById(query.ItemId);

        // Additional business rule checks
        if (item == null || item.SaleId != query.SaleId)
            return null;

        // Maps entity to output DTO with null-safe property access
        return new GetItemBySaleByIdOutput
        {
            Id = item.Id,
            MocktailId = item.MocktailId,
            MocktailName = item.Mocktail?.Name ?? "Unknown",
            Quantity = item.Quantity,
            UnitPrice = item.Mocktail?.Price ?? 0,
            ItemTotal = item.Quantity * (item.Mocktail?.Price ?? 0),
            SaleId = item.SaleId,
            SaleDate = item.Sale?.SaleDate ?? DateTime.MinValue
        };
    }
}