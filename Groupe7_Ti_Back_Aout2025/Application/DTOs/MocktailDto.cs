using System.Collections.Generic;

namespace Application.DTOs;

public class MocktailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Available { get; set; }  // Calculé dynamiquement
    public string Image { get; set; } = string.Empty;
    public List<MocktailIngredientDto> Ingredients { get; set; } = new List<MocktailIngredientDto>();
}

public class MocktailIngredientDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
} 