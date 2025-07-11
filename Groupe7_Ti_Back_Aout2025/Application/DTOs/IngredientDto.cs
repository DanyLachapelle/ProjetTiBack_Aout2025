namespace Application.DTOs;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Stock { get; set; }
    public decimal Limit { get; set; }
    public string Unit { get; set; } = string.Empty;
} 