using FluentAssertions;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

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
        mocktailIngredient.Unit.Should().BeNull(); // La valeur par défaut est null, pas empty
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

    [Theory]
    [InlineData(0.01)]
    [InlineData(1)]
    [InlineData(100.5)]
    public void Quantity_ShouldAcceptPositiveValues(decimal validValue)
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        item.Quantity = validValue;

        // Assert
        item.Quantity.Should().Be(validValue);
    }

    [Theory]
    [InlineData("g")]
    [InlineData("l")]
    [InlineData("cl")]
    public void Unit_ShouldAcceptValidValues(string unit)
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        item.Unit = unit;

        // Assert
        item.Unit.Should().Be(unit);
    }

    [Theory]
    [InlineData("kg")]
    [InlineData("ml")]
    [InlineData("")]
    [InlineData(null)]
    public void Unit_ShouldNotAcceptInvalidValues(string unit)
    {
        // Arrange
        var item = new mocktail_ingredient();

        // Act
        Action act = () => item.Unit = unit;

        // Assert
        act.Should().Throw<ValidationException>();
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
        mocktailIngredient.MocktailId.Should().Be(1);
        mocktailIngredient.IngredientId.Should().Be(2);
    }

    [Fact]
    public void NavigationProperties_ShouldHandleNullValues()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();
        var mocktail = new Mocktail { Id = 1 };
        var ingredient = new Ingredient { Id = 2 };
        mocktailIngredient.Mocktail = mocktail;
        mocktailIngredient.Ingredient = ingredient;

        // Act
        mocktailIngredient.Mocktail = null;
        mocktailIngredient.Ingredient = null;

        // Assert
        mocktailIngredient.Mocktail.Should().BeNull();
        mocktailIngredient.Ingredient.Should().BeNull();
        mocktailIngredient.MocktailId.Should().Be(0);
        mocktailIngredient.IngredientId.Should().Be(0);
    }

    [Fact]
    public void UpdateForeignKeys_ShouldSyncIdsWithNavigationProperties()
    {
        // Arrange
        var mocktail = new Mocktail { Id = 10 };
        var ingredient = new Ingredient { Id = 20 };
        var mocktailIngredient = new mocktail_ingredient
        {
            Mocktail = mocktail,
            Ingredient = ingredient
        };

        // Act
        mocktailIngredient.UpdateForeignKeys();

        // Assert
        mocktailIngredient.MocktailId.Should().Be(10);
        mocktailIngredient.IngredientId.Should().Be(20);
    }

    [Fact]
    public void UpdateForeignKeys_ShouldHandleNullNavigationProperties()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();

        // Act
        mocktailIngredient.UpdateForeignKeys();

        // Assert
        mocktailIngredient.MocktailId.Should().Be(0);
        mocktailIngredient.IngredientId.Should().Be(0);
    }

    [Fact]
    public void UpdateForeignKeys_ShouldUpdateIdsWhenNavigationPropertiesChange()
    {
        // Arrange
        var mocktail1 = new Mocktail { Id = 10 };
        var ingredient1 = new Ingredient { Id = 20 };
        var mocktailIngredient = new mocktail_ingredient
        {
            Mocktail = mocktail1,
            Ingredient = ingredient1
        };

        // Act - Change IDs of navigation properties
        mocktail1.Id = 30;
        ingredient1.Id = 40;
        mocktailIngredient.UpdateForeignKeys();

        // Assert
        mocktailIngredient.MocktailId.Should().Be(30);
        mocktailIngredient.IngredientId.Should().Be(40);
    }

    [Fact]
    public void SettingMocktail_ShouldUpdateMocktailId()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();
        var mocktail = new Mocktail { Id = 15 };

        // Act
        mocktailIngredient.Mocktail = mocktail;

        // Assert
        mocktailIngredient.MocktailId.Should().Be(15);
    }

    [Fact]
    public void SettingIngredient_ShouldUpdateIngredientId()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();
        var ingredient = new Ingredient { Id = 25 };

        // Act
        mocktailIngredient.Ingredient = ingredient;

        // Assert
        mocktailIngredient.IngredientId.Should().Be(25);
    }

    [Fact]
    public void SettingNullMocktail_ShouldSetMocktailIdToZero()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient { Mocktail = new Mocktail() };

        // Act
        mocktailIngredient.Mocktail = null;

        // Assert
        mocktailIngredient.MocktailId.Should().Be(0);
    }

    [Fact]
    public void SettingNullIngredient_ShouldSetIngredientIdToZero()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient { Ingredient = new Ingredient() };

        // Act
        mocktailIngredient.Ingredient = null;

        // Assert
        mocktailIngredient.IngredientId.Should().Be(0);
    }

    [Fact]
    public void Quantity_ShouldMaintainValueAfterValidAssignment()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();
        var originalQuantity = 10.5m;

        // Act
        mocktailIngredient.Quantity = originalQuantity;
        mocktailIngredient.Quantity = 15.75m; // Change to another valid value

        // Assert
        mocktailIngredient.Quantity.Should().Be(15.75m);
    }

    [Fact]
    public void Unit_ShouldMaintainValueAfterValidAssignment()
    {
        // Arrange
        var mocktailIngredient = new mocktail_ingredient();
        var originalUnit = "g";

        // Act
        mocktailIngredient.Unit = originalUnit;
        mocktailIngredient.Unit = "l"; // Change to another valid value

        // Assert
        mocktailIngredient.Unit.Should().Be("l");
    }
}