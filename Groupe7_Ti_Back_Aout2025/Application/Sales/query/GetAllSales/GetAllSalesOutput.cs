using Application.Sales.query.GetSalesByDate;

namespace Application.Sales.query.GetAllSales;

public class GetAllSalesOutput
{
    public List<SaleDto> sales { get; set; } = new List<SaleDto>();
}