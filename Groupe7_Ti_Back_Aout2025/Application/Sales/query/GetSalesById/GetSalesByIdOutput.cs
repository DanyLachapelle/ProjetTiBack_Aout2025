namespace Application.Sales.query.GetSalesById;

public class GetSalesByIdOutput
{
    public int id { get; set; }
    public string tableNumber { get; set; } = string.Empty;
    public decimal totalAmount { get; set; }
    public string saleDate { get; set; } = string.Empty;
    public string status { get; set; } = "Pending";
    public int order_timer { get; set; } = 15;
    public List<SaleItemOutput> items { get; set; } = new();
}

public class SaleItemOutput
{
    public int id { get; set; }
    public int mocktailId { get; set; }
    public string mocktailName { get; set; } = string.Empty;
    public int quantity { get; set; }
    public decimal unitPrice { get; set; }
    public decimal itemTotal { get; set; }
}