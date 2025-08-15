using Application.DTOs;
using Application.Sales.query.GetSalesByDate;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetAllSales;

public class GetAllSalesHandler : IQueryHandler<GetAllSalesQuery, GetAllSalesOutput>
{
    private readonly ISaleRepository _saleRepository;
    
    public GetAllSalesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
    
    public GetAllSalesOutput Handle(GetAllSalesQuery query)
    {
        try
        {
            // 1. Get all sales from repository (simple method)
            var sales = _saleRepository.GetAllSales();
            
            Console.WriteLine($"GetAllSalesHandler: {sales.Count()} sales retrieved");

            // 2. For each sale, get its items separately
            var salesWithItems = new List<SaleDto>();
            
            foreach (var sale in sales)
            {
                try
                {
                    // Get items for this specific sale
                    var saleItems = _saleRepository.GetSaleItemsBySaleId(sale.Id);
                    
                    var saleDto = new SaleDto
                    {
                        Id = sale.Id,
                        TableNumber = sale.TableNumber ?? string.Empty,
                        TotalAmount = sale.TotalAmount,
                        SaleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Status = sale.Status ?? "Pending",
                        OrderTimer = sale.OrderTimer,
                        Items = saleItems?.Select(i => new SaleItemDto
                        {
                            Id = i.Id,
                            MocktailId = i.MocktailId,
                            MocktailName = i.Mocktail?.Name ?? "Unknown Mocktail",
                            Quantity = i.Quantity,
                            UnitPrice = i.Mocktail?.Price ?? 0,
                            ItemTotal = i.ItemTotal
                        }).ToList() ?? new List<SaleItemDto>()
                    };
                    
                    salesWithItems.Add(saleDto);
                }
                catch (Exception itemEx)
                {
                    Console.WriteLine($"Error loading items for sale {sale.Id}: {itemEx.Message}");
                    // Create sale without items in case of error
                    var saleDto = new SaleDto
                    {
                        Id = sale.Id,
                        TableNumber = sale.TableNumber ?? string.Empty,
                        TotalAmount = sale.TotalAmount,
                        SaleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Status = sale.Status ?? "Pending",
                        OrderTimer = sale.OrderTimer,
                        Items = new List<SaleItemDto>()
                    };
                    
                    salesWithItems.Add(saleDto);
                }
            }

            // 3. Prepare response
            var output = new GetAllSalesOutput
            {
                Sales = salesWithItems
            };

            Console.WriteLine($"GetAllSalesHandler: {output.Sales.Count} sales in response");
            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllSalesHandler: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}