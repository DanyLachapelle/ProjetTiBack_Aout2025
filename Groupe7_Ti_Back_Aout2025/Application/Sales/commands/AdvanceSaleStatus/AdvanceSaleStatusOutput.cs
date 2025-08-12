namespace Application.Sales.commands.AdvanceSaleStatus;

public class AdvanceSaleStatusOutput
{
    public int saleId { get; set; }
    public string newStatus { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
}
