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
                        id = sale.Id,
                        tableNumber = sale.TableNumber ?? string.Empty,
                        totalAmount = sale.TotalAmount,
                        saleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        status = sale.status ?? "Pending",
                        order_timer = sale.order_timer,
                        items = saleItems?.Select(i => new SaleItemDto
                        {
                            id = i.Id,
                            mocktailId = i.MocktailId,
                            mocktailName = i.Mocktail?.name ?? "Unknown Mocktail",
                            quantity = i.Quantity,
                            unitPrice = i.Mocktail?.price ?? 0,
                            itemTotal = i.ItemTotal
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
                        id = sale.Id,
                        tableNumber = sale.TableNumber ?? string.Empty,
                        totalAmount = sale.TotalAmount,
                        saleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        status = sale.status ?? "Pending",
                        order_timer = sale.order_timer,
                        items = new List<SaleItemDto>()
                    };
                    
                    salesWithItems.Add(saleDto);
                }
            }

            // 3. Préparer la réponse
            var output = new GetAllSalesOutput
            {
                sales = salesWithItems
            };

            Console.WriteLine($"GetAllSalesHandler: {output.sales.Count} ventes dans la réponse");
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