namespace Application.DTOs;

public class UpdateSaleRequest
{
    public int ItemId { get; set; }
    public int? NewMocktailId { get; set; }
    public int? NewQuantity { get; set; }
    public decimal? NewUnitPrice { get; set; }
    
}