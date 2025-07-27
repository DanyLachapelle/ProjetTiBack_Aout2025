using System;

namespace Application.DTOs;

public class IngredientDto
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
    
    public string allergen { get; set; } = "none"; 
    public DateTime? last_modified_at { get; set; } 
    
    public bool NeedsRestock => quantity <= restock_threshold;
    
    public override string ToString() 
    {
        return $"{name} - {quantity}{unit} {(NeedsRestock ? "(Besoin réappro)" : "")}";
    }
} 