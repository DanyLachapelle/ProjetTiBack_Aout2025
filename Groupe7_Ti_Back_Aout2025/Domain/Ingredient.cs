using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class Ingredient
{ 
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("quantity")]
    public decimal Quantity { get; set; }
    [Column("restock_threshold")]
    public decimal RestockThreshold { get; set; }
    [Column("unit")]
    public string Unit { get; set; } = string.Empty;
    
    [Column("allergen")]
    public string Allergen { get; set; } = "none";
    [Column("last_modified_at")]
    public DateTime? LastModifiedAt { get; set; } 

    // Navigation property pour les mocktails
    public virtual ICollection<mocktail_ingredient> MocktailIngredients { get; set; } = new List<mocktail_ingredient>();
    
    // Méthode métier pour ajouter de la quantité
    public void AddQuantity(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount to add must be positive", nameof(amount));
        }

        Quantity += amount;
        LastModifiedAt = DateTime.Now;
    }
    
    public void DecreaseQuantity(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount to decrease must be positive", nameof(amount));
        }

        if (Quantity < amount)
        {
            throw new InvalidOperationException("Insufficient quantity to decrease");
        }

        Quantity -= amount;
        LastModifiedAt = DateTime.Now;
    }

    
    public bool NeedsRestock()
    {
        return Quantity <= RestockThreshold;
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