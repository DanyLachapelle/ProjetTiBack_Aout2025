using FluentAssertions;
using Domain;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.Unit.Domain;

public class MocktailTests
{
    [Fact]
    public void Mocktail_Initialization_ShouldSetDefaultValues()
    {
        // Arrange & Act
        var mocktail = new Mocktail();

        // Assert
        mocktail.Id.Should().Be(0);
        mocktail.Name.Should().BeEmpty();
        mocktail.Description.Should().BeEmpty();
        mocktail.Price.Should().Be(0);
        mocktail.Image.Should().BeNull();
        mocktail.ForceAvailable.Should().BeNull(); // La valeur par défaut est null, pas false
        mocktail.MocktailIngredients.Should().NotBeNull();
        mocktail.MocktailIngredients.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Virgin Mojito", "Refreshing mint drink", 5.99, "mojito.jpg", true)]
    [InlineData("Fruit Punch", "Mixed fruit beverage", 4.50, null, false)]
    [InlineData("Lemonade", "Fresh lemon drink", 0, "", null)]
    public void Mocktail_PropertyAssignment_ShouldWorkCorrectly(
        string name, string description, decimal price, string image, bool? forceAvailable)
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act
        mocktail.Name = name;
        mocktail.Description = description;
        mocktail.Price = price;
        mocktail.Image = image;
        mocktail.ForceAvailable = forceAvailable;

        // Assert
        mocktail.Name.Should().Be(name);
        mocktail.Description.Should().Be(description);
        mocktail.Price.Should().Be(price);
        mocktail.Image.Should().Be(image);
        mocktail.ForceAvailable.Should().Be(forceAvailable ?? false);
    }

    [Fact]
    public void MocktailIngredients_ShouldAllowAddingIngredients()
    {
        // Arrange
        var mocktail = new Mocktail();
        var ingredient1 = new mocktail_ingredient();
        var ingredient2 = new mocktail_ingredient();

        // Act
        mocktail.MocktailIngredients.Add(ingredient1);
        mocktail.MocktailIngredients.Add(ingredient2);

        // Assert
        mocktail.MocktailIngredients.Should().HaveCount(2);
        mocktail.MocktailIngredients.Should().Contain(ingredient1);
        mocktail.MocktailIngredients.Should().Contain(ingredient2);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    [InlineData(null, false)]
    public void ForceAvailable_ShouldHandleAllValues(bool? input, bool expected)
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act
        mocktail.ForceAvailable = input;

        // Assert
        mocktail.ForceAvailable.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(100.50)]
    public void Price_ShouldAcceptNonNegativeValues(decimal price)
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act
        mocktail.Price = price;

        // Assert
        mocktail.Price.Should().Be(price);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(-100.50)]
    public void Price_WithNegativeValues_ShouldThrowException(decimal price)
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act & Assert
        var action = () => mocktail.Price = price;
        action.Should().Throw<ValidationException>()
              .WithMessage("Price cannot be negative");
    }

    [Fact]
    public void Price_ShouldMaintainValueAfterValidAssignment()
    {
        // Arrange
        var mocktail = new Mocktail();
        var originalPrice = 10.50m;

        // Act
        mocktail.Price = originalPrice;
        mocktail.Price = 15.75m; // Change to another valid value

        // Assert
        mocktail.Price.Should().Be(15.75m);
    }

    [Fact]
    public void MocktailIngredients_ShouldBeEmptyByDefault()
    {
        // Arrange & Act
        var mocktail = new Mocktail();

        // Assert
        mocktail.MocktailIngredients.Should().NotBeNull();
        mocktail.MocktailIngredients.Should().BeEmpty();
    }

    [Fact]
    public void MocktailIngredients_ShouldAllowRemovingIngredients()
    {
        // Arrange
        var mocktail = new Mocktail();
        var ingredient1 = new mocktail_ingredient();
        var ingredient2 = new mocktail_ingredient();
        mocktail.MocktailIngredients.Add(ingredient1);
        mocktail.MocktailIngredients.Add(ingredient2);

        // Act
        mocktail.MocktailIngredients.Remove(ingredient1);

        // Assert
        mocktail.MocktailIngredients.Should().HaveCount(1);
        mocktail.MocktailIngredients.Should().Contain(ingredient2);
        mocktail.MocktailIngredients.Should().NotContain(ingredient1);
    }

    [Fact]
    public void MocktailIngredients_ShouldAllowClearingAllIngredients()
    {
        // Arrange
        var mocktail = new Mocktail();
        var ingredient1 = new mocktail_ingredient();
        var ingredient2 = new mocktail_ingredient();
        mocktail.MocktailIngredients.Add(ingredient1);
        mocktail.MocktailIngredients.Add(ingredient2);

        // Act
        mocktail.MocktailIngredients.Clear();

        // Assert
        mocktail.MocktailIngredients.Should().BeEmpty();
    }

    [Fact]
    public void Image_ShouldAcceptNullValue()
    {
        // Arrange
        var mocktail = new Mocktail { Image = "test.jpg" };

        // Act
        mocktail.Image = null;

        // Assert
        mocktail.Image.Should().BeNull();
    }

    [Fact]
    public void Image_ShouldAcceptEmptyString()
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act
        mocktail.Image = "";

        // Assert
        mocktail.Image.Should().Be("");
    }
}