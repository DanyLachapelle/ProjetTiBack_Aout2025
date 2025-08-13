using Application.DTOs;
using Application.Sales.query.GetSalesByDate;
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
        try
        {
            // 1. Récupérer toutes les ventes depuis le repository (méthode simple)
            var sales = _saleRepository.GetAllSales();
            
            Console.WriteLine($"GetAllSalesHandler: {sales.Count()} ventes récupérées");

            // 2. Pour chaque vente, récupérer ses items séparément 
            var salesWithItems = new List<SaleDto>();
            
            foreach (var sale in sales)
            {
                try
                {
                    // Récupérer les items de cette vente spécifiquement
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
                    Console.WriteLine($"Erreur lors du chargement des items pour la vente {sale.Id}: {itemEx.Message}");
                    // Créer la vente sans items en cas d'erreur
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

            // 3. Préparer la réponse
            var output = new GetAllSalesOutput
            {
                Sales = salesWithItems
            };

            Console.WriteLine($"GetAllSalesHandler: {output.Sales.Count} ventes dans la réponse");
            return output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur dans GetAllSalesHandler: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}