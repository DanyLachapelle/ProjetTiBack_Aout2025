namespace Application.Sales.commands.AdvanceSaleStatus;

public class AdvanceSaleStatusOutput
{
    public int saleId { get; set; }
    public string NewStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
