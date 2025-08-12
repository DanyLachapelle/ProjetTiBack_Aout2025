namespace Application.DTOs;

public class SaleItemDto
{
    public int id { get; set; }
    public int mocktailId { get; set; }
    public string mocktailName { get; set; } = string.Empty;
    public int quantity { get; set; }
    public decimal unitPrice { get; set; }
    public decimal itemTotal { get; set; }
}