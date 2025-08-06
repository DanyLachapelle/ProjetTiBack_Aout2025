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
    
    public string StockStatus 
    { 
        get
        {
            if (quantity < restock_threshold * 0.8m)
                return "critical";
            if (quantity > restock_threshold * 1.5m)
                return "good";
            return "warning";
        }
    }

    public bool NeedsRestock => quantity <= restock_threshold;
    
    public override string ToString() 
    {
        return $"{name} - {quantity}{unit} {(NeedsRestock ? "(Besoin réappro)" : "")}";
    }
    
   

} 