using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;


public class mocktail_ingredient
{
    private Mocktail _mocktail;
    private Ingredient _ingredient;
    private decimal _quantity;
    private string _unit;


    [Column("id")]
    public int Id { get; set; }
    
    [Column("mocktail_id")]
    public int MocktailId { get; set; }
    
    [Column("ingredient_id")]
    public int IngredientId { get; set; }
    
    [Column("quantity")]
    public decimal Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new ValidationException("Quantity must be positive");
            _quantity = value;
        }
    }
    
    [Column("unit")]
    public string Unit
    {
        get => _unit;
        set
        {
            if (!Ingredient.ValidUnits.Contains(value))
                throw new ValidationException($"Invalid unit. Valid values are: {string.Join(", ", Ingredient.ValidUnits)}");
            _unit = value;
        }
    }    

    
    public virtual Mocktail Mocktail
    {
        get => _mocktail;
        set
        {
            _mocktail = value;
            MocktailId = value?.Id ?? 0; // Synchronise l'ID quand on assigne l'objet
        }
    }

    public virtual Ingredient Ingredient
    {
        get => _ingredient;
        set
        {
            _ingredient = value;
            IngredientId = value?.Id ?? 0; // Synchronise l'ID quand on assigne l'objet
        }
    }

    public void UpdateForeignKeys()
    {
        // Synchronise les IDs si les objets sont déjà assignés
        if (_mocktail != null) MocktailId = _mocktail.Id;
        if (_ingredient != null) IngredientId = _ingredient.Id;
    }
} 