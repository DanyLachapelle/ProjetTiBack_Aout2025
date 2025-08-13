using FluentAssertions;
using Domain;
using System.Linq;

namespace Tests.Unit.Domain;

public class MocktailTests
{
    [Fact]
    public void Mocktail_Initialization_ShouldSetDefaultValues()
    {
        // Arrange & Act
        var mocktail = new Mocktail();

        // Assert
        mocktail.Name.Should().BeEmpty();
        mocktail.Description.Should().BeEmpty();
        mocktail.Price.Should().Be(0);
        mocktail.Image.Should().BeNull();
        mocktail.ForceAvailable.Should().BeFalse();
        mocktail.MocktailIngredients.Should().NotBeNull();
        mocktail.MocktailIngredients.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Virgin Mojito", "Refreshing mint drink", 5.99, "mojito.jpg", true)]
    [InlineData("Fruit Punch", "Mixed fruit beverage", 4.50, null, false)]
    public void Mocktail_PropertyAssignment_ShouldWorkCorrectly(
        string name, string description, decimal price, string image, bool forceAvailable)
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
        mocktail.ForceAvailable.Should().Be(forceAvailable);
    }

    [Fact]
    public void MocktailIngredients_ShouldAllowAddingIngredients()
    {
        // Arrange
        var mocktail = new Mocktail();
        var ingredient1 = new mocktail_ingredient { /* initialisez selon votre structure */ };
        var ingredient2 = new mocktail_ingredient { /* initialisez selon votre structure */ };

        // Act
        mocktail.MocktailIngredients.Add(ingredient1);
        mocktail.MocktailIngredients.Add(ingredient2);

        // Assert
        mocktail.MocktailIngredients.Should().HaveCount(2);
        mocktail.MocktailIngredients.Should().Contain(ingredient1);
        mocktail.MocktailIngredients.Should().Contain(ingredient2);
    }

    [Fact]
    public void ForceAvailable_WhenNull_ShouldDefaultToFalse()
    {
        // Arrange
        var mocktail = new Mocktail { ForceAvailable = null };

        // Act & Assert
        mocktail.ForceAvailable.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(0.01, true)]
    [InlineData(-1, false)]
    public void Price_Validation_ShouldAcceptOnlyNonNegativeValues(decimal price, bool isValid)
    {
        // Arrange
        var mocktail = new Mocktail();

        // Act
        if (isValid)
        {
            mocktail.Price = price;
            mocktail.Price.Should().Be(price);
        }
        else
        {
            var act = () => mocktail.Price = price;
            act.Should().Throw<System.Exception>(); // Adaptez selon votre logique de validation
        }
    }
}