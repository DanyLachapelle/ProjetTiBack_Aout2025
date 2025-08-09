using Application.DTOs;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetAllSales;

public class GetAllSalesHandler:IQueryHandler<GetAllSalesQuery, GetAllSalesOutput>
{
    private readonly ISaleRepository _saleRepository;
    
public GetAllSalesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
    public GetAllSalesOutput Handle(GetAllSalesQuery query)
    {
        // 1. Récupérer toutes les ventes depuis le repository
        var sales = _saleRepository.GetAllSales();

        // 2. Préparer la réponse
        var output = new GetAllSalesOutput
        {
            Sales = sales.Select(s => new SaleDto
            {
                Id = s.Id,
                TableNumber = s.TableNumber,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate,
                Status = s.status,
                                    order_timer = s.order_timer ?? 15,
                Items = s.SaleItems?
                    .Where(si => si.MocktailId.HasValue && si.Mocktail != null)
                    .Select(i => new SaleItemDto
                    {
                        Id = i.Id,
                        MocktailId = i.MocktailId,
                        MocktailName = i.Mocktail?.name ?? "Inconnu",
                        Quantity = i.Quantity,
                        UnitPrice = i.Mocktail?.price ?? 0,
                        ItemTotal = i.ItemTotal
                    })
                    .ToList() ?? new List<SaleItemDto>()
            })
            .OrderByDescending(s => s.SaleDate)
            .ToList()
        };

        return output;
    }
}