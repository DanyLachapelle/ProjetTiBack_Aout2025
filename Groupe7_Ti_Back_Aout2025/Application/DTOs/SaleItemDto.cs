namespace Application.DTOs;

public class SaleItemDto
{
    public int Id { get; set; }
    public int? MocktailId { get; set; } // Nullable pour correspondre au domaine
    public string MocktailName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ItemTotal { get; set; }
}