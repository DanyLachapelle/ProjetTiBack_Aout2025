using FluentAssertions;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
        mocktailIngredient.Unit.Should().BeEmpty();
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

    [Fact]
    public void Quantity_ShouldNotAcceptNegativeValues()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();

        // Act & Assert
        mocktailIngredient.Invoking(x => x.Quantity = -1)
            .Should().Throw<ValidationException>()
            .WithMessage("Quantity cannot be negative");
    }

    [Theory]
    [InlineData("g", true)]
    [InlineData("cl", true)]
    [InlineData("l", true)]
    [InlineData("kg", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Unit_ShouldOnlyAcceptValidValues(string unit, bool isValid)
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();

        // Act & Assert
        if (isValid)
        {
            mocktailIngredient.Unit = unit;
            mocktailIngredient.Unit.Should().Be(unit);
        }
        else
        {
            mocktailIngredient.Invoking(x => x.Unit = unit)
                .Should().Throw<ValidationException>();
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

    [Fact]
    public void JsonIgnoreAttribute_ShouldPreventSerializationOfMocktail()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient
        {
            Id = 1,
            Mocktail = new Mocktail { Id = 2 },
            Ingredient = new Ingredient { Id = 3 }
        };

        // Act
        var json = JsonSerializer.Serialize(mocktailIngredient);
        var deserialized = JsonSerializer.Deserialize<mocktail_ingredient>(json);

        // Assert
        json.Should().NotContain("Mocktail");
        deserialized.Mocktail.Should().BeNull();
        deserialized.Ingredient.Should().NotBeNull();
    }

    [Fact]
    public void ForeignKeyProperties_ShouldSyncWithNavigationProperties()
    {
        // Arrange
        var mocktail = new Mocktail { Id = 10 };
        var ingredient = new Ingredient { Id = 20 };
        var mocktailIngredient = new mocktail_ingredient();

        // Act
        mocktailIngredient.Mocktail = mocktail;
        mocktailIngredient.Ingredient = ingredient;

        // Assert
        mocktailIngredient.MocktailId.Should().Be(10);
        mocktailIngredient.IngredientId.Should().Be(20);

        // Act & Assert (changement via ID)
        mocktailIngredient.MocktailId = 30;
        mocktailIngredient.IngredientId = 40;
        mocktailIngredient.Mocktail.Id.Should().Be(30);
        mocktailIngredient.Ingredient.Id.Should().Be(40);
    }
}