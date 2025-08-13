using FluentAssertions;
using Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Xunit;

namespace Tests.Unit.Domain;

public class SaleItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var item = new SaleItem();

        // Assert
        item.Id.Should().Be(0);
        item.SaleId.Should().Be(0);
        item.MocktailId.Should().Be(0);
        item.Quantity.Should().Be(0);
        item.ItemTotal.Should().Be(0);
        item.TotalAmount.Should().Be(0);
        item.Sale.Should().BeNull();
        item.Mocktail.Should().BeNull();
    }

    [Theory]
    [InlineData(1, 10, 5, 2, 25.50)]
    [InlineData(2, 20, 3, 1, 0)]
    public void PropertyAssignment_ShouldWorkCorrectly(
        int id, int saleId, int mocktailId, int quantity, decimal itemTotal)
    {
        // Arrange
        var item = new SaleItem();
        var mocktail = new Mocktail();
        var sale = new Sale();

        // Act
        item.Id = id;
        item.SaleId = saleId;
        item.MocktailId = mocktailId;
        item.Quantity = quantity;
        item.ItemTotal = itemTotal;
        item.Mocktail = mocktail;
        item.Sale = sale;

        // Assert
        item.Id.Should().Be(id);
        item.SaleId.Should().Be(saleId);
        item.MocktailId.Should().Be(mocktailId);
        item.Quantity.Should().Be(quantity);
        item.ItemTotal.Should().Be(itemTotal);
        item.Mocktail.Should().Be(mocktail);
        item.Sale.Should().Be(sale);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Quantity_ShouldNotAcceptNegativeValues(int quantity)
    {
        // Arrange
        var item = new SaleItem();

        // Act & Assert
        item.Invoking(x => x.Quantity = quantity)
            .Should().Throw<ValidationException>()
            .WithMessage("Quantity cannot be negative");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void ItemTotal_ShouldNotAcceptNegativeValues(decimal total)
    {
        // Arrange
        var item = new SaleItem();

        // Act & Assert
        item.Invoking(x => x.ItemTotal = total)
            .Should().Throw<ValidationException>()
            .WithMessage("ItemTotal cannot be negative");
    }

    [Fact]
    public void CalculateTotal_ShouldComputeCorrectAmount()
    {
        // Arrange
        var mocktail = new Mocktail { Price = 8.50m };
        var item = new SaleItem 
        { 
            Quantity = 3,
            Mocktail = mocktail
        };

        // Act
        item.CalculateTotal(); // Méthode à implémenter

        // Assert
        item.ItemTotal.Should().Be(25.50m);
        item.TotalAmount.Should().Be(25.50m);
    }

    [Fact]
    public void JsonIgnoreAttribute_ShouldPreventSaleSerialization()
    {
        // Arrange
        var item = new SaleItem
        {
            Id = 1,
            Sale = new Sale { Id = 10 },
            Mocktail = new Mocktail { Id = 20 }
        };

        // Act
        var json = JsonSerializer.Serialize(item);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json);

        // Assert
        json.Should().NotContain("Sale");
        deserialized.Sale.Should().BeNull();
        deserialized.Mocktail.Should().NotBeNull();
    }

    [Fact]
    public void NotMappedAttribute_ShouldExcludeTotalAmountFromPersistence()
    {
        // Arrange
        var item = new SaleItem { TotalAmount = 100 };

        // Act
        var json = JsonSerializer.Serialize(item);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json);

        // Assert
        deserialized.TotalAmount.Should().Be(0); // Non sérialisé
    }

    [Fact]
    public void ForeignKeyProperties_ShouldSyncWithNavigationProperties()
    {
        // Arrange
        var sale = new Sale { Id = 15 };
        var mocktail = new Mocktail { Id = 25 };
        var item = new SaleItem();

        // Act
        item.Sale = sale;
        item.Mocktail = mocktail;

        // Assert
        item.SaleId.Should().Be(15);
        item.MocktailId.Should().Be(25);

        // Test inverse
        item.SaleId = 30;
        item.MocktailId = 40;
        item.Sale.Id.Should().Be(30);
        item.Mocktail.Id.Should().Be(40);
    }
    [Fact]
    public void CalculateTotal_ShouldThrowWhenMocktailNotSet()
    {
        var item = new SaleItem { Quantity = 2 };
        item.Invoking(x => x.CalculateTotal())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Mocktail reference is required");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateTotal_ShouldRejectInvalidQuantities(int quantity)
    {
        var item = new SaleItem 
        { 
            Quantity = quantity,
            Mocktail = new Mocktail { Price = 10 }
        };
    
        item.Invoking(x => x.CalculateTotal())
            .Should().Throw<ValidationException>()
            .WithMessage("Quantity must be positive");
    }

    [Fact]
    public void CalculateTotal_ShouldHandlePriceChanges()
    {
        var mocktail = new Mocktail { Price = 10 };
        var item = new SaleItem { Mocktail = mocktail, Quantity = 3 };
    
        item.CalculateTotal();
        mocktail.Price = 15; // Changement de prix
    
        item.CalculateTotal(); // Recalcul
        item.ItemTotal.Should().Be(45);
    }
}
