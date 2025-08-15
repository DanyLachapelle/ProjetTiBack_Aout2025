using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class Mocktail
{
    private decimal _price;
    private bool? _forceAvailable;


    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("price")]
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ValidationException("Price cannot be negative");
            _price = value;
        }
    }
    [Column("image")]
    public string? Image { get; set; }
    [Column("forceAvailable")]
    public bool? ForceAvailable
    {
        get => _forceAvailable;
        set => _forceAvailable = value ?? false; // Convertit null en false
    } 

    // Navigation property pour les ingrédients
    public virtual ICollection<mocktail_ingredient> MocktailIngredients { get; set; } = new List<mocktail_ingredient>();
} 