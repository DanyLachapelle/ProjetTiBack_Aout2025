using FluentAssertions;
using Domain;
using System;
using Xunit;

namespace Tests.Unit.Domain;

public class IngredientTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var ingredient = new Ingredient();

        // Assert
        ingredient.Id.Should().Be(0);
        ingredient.Name.Should().BeEmpty();
        ingredient.Quantity.Should().Be(0);
        ingredient.RestockThreshold.Should().Be(0);
        ingredient.Unit.Should().BeEmpty();
        ingredient.Allergen.Should().Be("none");
        ingredient.LastModifiedAt.Should().BeNull();
        ingredient.MocktailIngredients.Should().NotBeNull();
        ingredient.MocktailIngredients.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Sucre", "g", "none")]
    [InlineData("Lait", "l", "milk")]
    [InlineData("Noix", "g", "nuts")]
    public void PropertyAssignment_ShouldWorkCorrectly(string name, string unit, string allergen)
    {
        // Arrange
        var ingredient = new Ingredient();

        // Act
        ingredient.Name = name;
        ingredient.Unit = unit;
        ingredient.Allergen = allergen;

        // Assert
        ingredient.Name.Should().Be(name);
        ingredient.Unit.Should().Be(unit);
        ingredient.Allergen.Should().Be(allergen);
    }

    [Fact]
    public void AddQuantity_WithPositiveAmount_ShouldIncreaseQuantity()
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        
        // Act
        ingredient.AddQuantity(50);
        
        // Assert
        ingredient.Quantity.Should().Be(150);
        ingredient.LastModifiedAt.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void AddQuantity_WithNonPositiveAmount_ShouldThrowException(decimal amount)
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        
        // Act & Assert
        var action = () => ingredient.AddQuantity(amount);
        action.Should().Throw<ArgumentException>()
              .WithMessage("Amount to add must be positive*");
    }

    [Fact]
    public void DecreaseQuantity_WithValidAmount_ShouldDecreaseQuantity()
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        
        // Act
        ingredient.DecreaseQuantity(30);
        
        // Assert
        ingredient.Quantity.Should().Be(70);
        ingredient.LastModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public void DecreaseQuantity_WithInsufficientStock_ShouldThrowException()
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 10 };
        
        // Act & Assert
        var action = () => ingredient.DecreaseQuantity(20);
        action.Should().Throw<InvalidOperationException>()
              .WithMessage("Insufficient quantity to decrease");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5.5)]
    public void DecreaseQuantity_WithNonPositiveAmount_ShouldThrowException(decimal amount)
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        
        // Act & Assert
        var action = () => ingredient.DecreaseQuantity(amount);
        action.Should().Throw<ArgumentException>()
              .WithMessage("Amount to decrease must be positive*");
    }

    [Theory]
    [InlineData(5, 10, true)]   // Quantity < Threshold
    [InlineData(10, 10, true)]  // Quantity = Threshold
    [InlineData(15, 10, false)] // Quantity > Threshold
    public void NeedsRestock_ShouldReturnCorrectValue(decimal quantity, decimal threshold, bool expected)
    {
        // Arrange
        var ingredient = new Ingredient 
        { 
            Quantity = quantity, 
            RestockThreshold = threshold 
        };
        
        // Act
        var result = ingredient.NeedsRestock();
        
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ValidUnits_ShouldContainExpectedValues()
    {
        // Act & Assert
        Ingredient.ValidUnits.Should().Contain("g");
        Ingredient.ValidUnits.Should().Contain("l");
        Ingredient.ValidUnits.Should().Contain("cl");
        Ingredient.ValidUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ValidAllergens_ShouldContainExpectedValues()
    {
        // Act & Assert
        Ingredient.ValidAllergens.Should().Contain("none");
        Ingredient.ValidAllergens.Should().Contain("gluten");
        Ingredient.ValidAllergens.Should().Contain("milk");
        Ingredient.ValidAllergens.Should().Contain("nuts");
        Ingredient.ValidAllergens.Should().HaveCount(15);
    }

    [Fact]
    public void MocktailIngredients_ShouldAllowAddingIngredients()
    {
        // Arrange
        var ingredient = new Ingredient();
        var mocktailIngredient1 = new mocktail_ingredient();
        var mocktailIngredient2 = new mocktail_ingredient();

        // Act
        ingredient.MocktailIngredients.Add(mocktailIngredient1);
        ingredient.MocktailIngredients.Add(mocktailIngredient2);

        // Assert
        ingredient.MocktailIngredients.Should().HaveCount(2);
        ingredient.MocktailIngredients.Should().Contain(mocktailIngredient1);
        ingredient.MocktailIngredients.Should().Contain(mocktailIngredient2);
    }

    [Fact]
    public void LastModifiedAt_ShouldBeUpdatedOnQuantityChanges()
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        var beforeAdd = DateTime.Now;

        // Act
        ingredient.AddQuantity(50);
        var afterAdd = DateTime.Now;

        // Assert
        ingredient.LastModifiedAt.Should().BeOnOrAfter(beforeAdd);
        ingredient.LastModifiedAt.Should().BeOnOrBefore(afterAdd);

        // Act
        ingredient.DecreaseQuantity(30);
        var afterDecrease = DateTime.Now;

        // Assert
        ingredient.LastModifiedAt.Should().BeOnOrAfter(afterAdd);
        ingredient.LastModifiedAt.Should().BeOnOrBefore(afterDecrease);
    }
}
