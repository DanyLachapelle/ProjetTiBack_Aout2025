using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class Ingredient
{ 
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
    
    public string allergen { get; set; } = "none";
    public DateTime? last_modified_at { get; set; }

    // Navigation property pour les mocktails
    public virtual ICollection<mocktail_ingredient> MocktailIngredients { get; set; } = new List<mocktail_ingredient>();
    
    // Méthode métier pour ajouter de la quantité
    public void AddQuantity(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount to add must be positive", nameof(amount));
        }

        quantity += amount;
        last_modified_at = DateTime.Now;
    }
    
    public bool NeedsRestock()
    {
        return quantity <= restock_threshold;
    }

    // Validation des valeurs possibles pour Unit
    public static readonly string[] ValidUnits = { "g", "l", "cl" };

    // Validation des valeurs possibles pour Allergen
    public static readonly string[] ValidAllergens = 
    {
        "none", "gluten", "crustaceans", "eggs", "fish", "peanuts",
        "soybeans", "milk", "nuts", "celery", "mustard", "sesame",
        "sulphites", "lupin", "molluscs"
    };
} 