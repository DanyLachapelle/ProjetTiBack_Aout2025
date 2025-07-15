using System;

namespace Application.DTOs;

public class IngredientDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
    public DateTime? last_modified_at { get; set; } 
} 