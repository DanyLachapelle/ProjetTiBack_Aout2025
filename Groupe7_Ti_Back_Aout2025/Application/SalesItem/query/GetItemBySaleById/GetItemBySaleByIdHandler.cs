using Application.Utils;
using Infrastructure.Sale;
using Infrastructure.User.Sale;

namespace Application.SalesItem.query.GetItemBySaleById;

public class GetItemBySaleByIdHandler : IQueryHandler<GetItemBySaleByIdQuery, GetItemBySaleByIdOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetItemBySaleByIdHandler(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public GetItemBySaleByIdOutput Handle(GetItemBySaleByIdQuery query)
    {
        // Validation
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        // Utilisation directe du repository existant
        var item = _saleItemRepository.GetById(query.ItemId);

        // Vérifications supplémentaires
        if (item == null || item.SaleId != query.SaleId)
            return null;

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