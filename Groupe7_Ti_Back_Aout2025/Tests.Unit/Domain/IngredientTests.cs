using FluentAssertions;
using Domain;

namespace Tests.Unit.Domain;

public class IngredientTests
{
    [Fact]
    public void AddQuantity_WithPositiveAmount_ShouldIncreaseQuantity()
    {
        // Arrange
        var ingredient = new Ingredient { Quantity = 100 };
        
        // Act
        ingredient.AddQuantity(50);
        
        // Assert
        ingredient.Quantity.Should().Be(150);
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
    
    [Fact]
    public void NeedsRestock_WhenQuantityBelowThreshold_ShouldReturnTrue()
    {
        // Arrange
        var ingredient = new Ingredient 
        { 
            Quantity = 5, 
            RestockThreshold = 10 
        };
        
        // Act
        var result = ingredient.NeedsRestock();
        
        // Assert
        result.Should().BeTrue();
    }
}
