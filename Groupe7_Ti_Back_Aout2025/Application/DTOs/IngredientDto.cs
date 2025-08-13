using System;

namespace Application.DTOs;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal RestockThreshold { get; set; }
    public string Unit { get; set; } = string.Empty;
    
    public string Allergen { get; set; } = "none"; 
    public DateTime? LastModifiedAt { get; set; } 
    
    public string StockStatus 
    { 
        get
        {
            if (Quantity < RestockThreshold * 0.8m)
                return "critical";
            if (Quantity > RestockThreshold * 1.5m)
                return "good";
            return "warning";
        }
    }

    public bool NeedsRestock => Quantity <= RestockThreshold;
    
    public override string ToString() 
    {
        return $"{Name} - {Quantity}{Unit} {(NeedsRestock ? "(Needs restock)" : "")}";
    }
    
   

} 