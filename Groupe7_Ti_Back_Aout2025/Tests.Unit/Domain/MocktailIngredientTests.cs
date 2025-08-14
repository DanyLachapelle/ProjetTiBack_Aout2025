using FluentAssertions;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tests.Unit.Domain;

public class MocktailIngredientTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var mocktailIngredient = new mocktail_ingredient();

        // Assert
        mocktailIngredient.Id.Should().Be(0);
        mocktailIngredient.MocktailId.Should().Be(0);
        mocktailIngredient.IngredientId.Should().Be(0);
        mocktailIngredient.Quantity.Should().Be(0);
        mocktailIngredient.Mocktail.Should().BeNull();
        mocktailIngredient.Ingredient.Should().BeNull();
    }

    [Theory]
    [InlineData(1, 2, 3, 10.5, "g")]
    [InlineData(4, 5, 6, 0.75, "cl")]
    public void PropertyAssignment_ShouldWorkCorrectly(
        int id, int mocktailId, int ingredientId, decimal quantity, string unit)
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();

        // Act
        mocktailIngredient.Id = id;
        mocktailIngredient.MocktailId = mocktailId;
        mocktailIngredient.IngredientId = ingredientId;
        mocktailIngredient.Quantity = quantity;
        mocktailIngredient.Unit = unit;

        // Assert
        mocktailIngredient.Id.Should().Be(id);
        mocktailIngredient.MocktailId.Should().Be(mocktailId);
        mocktailIngredient.IngredientId.Should().Be(ingredientId);
        mocktailIngredient.Quantity.Should().Be(quantity);
        mocktailIngredient.Unit.Should().Be(unit);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(0)]
    public void Quantity_ShouldNotAcceptNonPositiveValues(decimal invalidValue)
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        Action act = () => item.Quantity = invalidValue;

        // Assert
        act.Should()
            .Throw<ValidationException>()
            .WithMessage("Quantity must be positive");
    }

    
    [Fact]
    public void Quantity_ShouldAcceptPositiveValues()
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        item.Quantity = 1;

        // Assert
        item.Quantity.Should().Be(1);
    }

    [Theory]
    [InlineData("g", true)]
    [InlineData("l", true)]
    [InlineData("cl", true)]
    [InlineData("kg", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Unit_ShouldOnlyAcceptValidValues(string unit, bool isValid)
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        Action act = () => item.Unit = unit;

        // Assert
        if (isValid)
        {
            act.Should().NotThrow();
            item.Unit.Should().Be(unit);
        }
        else
        {
            act.Should().Throw<ValidationException>();
        }
    }

    [Fact]
    public void NavigationProperties_ShouldBeAssignable()
    {
        // Arrange
        var mocktail = new Mocktail { Id = 1 };
        var ingredient = new Ingredient { Id = 2 };
        var mocktailIngredient = new mocktail_ingredient();

        // Act
        mocktailIngredient.Mocktail = mocktail;
        mocktailIngredient.Ingredient = ingredient;

        // Assert
        mocktailIngredient.Mocktail.Should().Be(mocktail);
        mocktailIngredient.Ingredient.Should().Be(ingredient);
        mocktailIngredient.MocktailId.Should().Be(1); // Vérifie la cohérence de l'ID
        mocktailIngredient.IngredientId.Should().Be(2);
    }
/*
    [Fact]
    public void JsonIgnoreAttribute_ShouldPreventSerializationOfNavigationProperties()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient
        {
            Id = 1,
            MocktailId = 2,
            IngredientId = 3,
            Mocktail = new Mocktail { Id = 2 },
            Ingredient = new Ingredient { Id = 3 }
        };

        // Act
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles, // Important pour les références circulaires
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    
        var json = JsonSerializer.Serialize(mocktailIngredient, options);
        var deserialized = JsonSerializer.Deserialize<mocktail_ingredient>(json, options);

        // Assert
        json.Should().NotContain("Mocktail");
        json.Should().NotContain("Ingredient"); // Si vous voulez aussi ignorer Ingredient
        deserialized.Mocktail.Should().BeNull();
        deserialized.Ingredient.Should().BeNull(); // Ou .NotBeNull() selon votre besoin
    
        // Vérifiez que les IDs sont bien sérialisés
        json.Should().Contain("\"mocktailId\":2");
        json.Should().Contain("\"ingredientId\":3");
    }
*/
    [Fact]
    public void ForeignKeyProperties_ShouldSyncWithNavigationProperties()
    {
        // Arrange
        var mocktail = new Mocktail { Id = 10 };
        var ingredient = new Ingredient { Id = 20 };
        var mocktailIngredient = new mocktail_ingredient();

        // Act - Assignation des objets de navigation
        mocktailIngredient.Mocktail = mocktail;
        mocktailIngredient.Ingredient = ingredient;

        // Assert - Vérifie que les IDs sont synchronisés
        mocktailIngredient.MocktailId.Should().Be(10);
        mocktailIngredient.IngredientId.Should().Be(20);

        // Act - Modification des IDs des objets parents
        mocktail.Id = 30;
        ingredient.Id = 40;
        mocktailIngredient.UpdateForeignKeys(); // Force la mise à jour

        // Assert - Vérifie que les IDs sont à jour
        mocktailIngredient.MocktailId.Should().Be(30);
        mocktailIngredient.IngredientId.Should().Be(40);
    }
}