using Application.DTOs;

namespace Application.Sales.query.GetAllSales;

public class GetAllSalesOutput
{
    public List<SaleDto> Sales { get; set; } = new List<SaleDto>();
}