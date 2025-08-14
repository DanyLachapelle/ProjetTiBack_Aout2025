using Domain;
using FluentAssertions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    [InlineData(2, 20, 0, 1, 0)] // MocktailId peut être 0
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
        item.TotalAmount.Should().Be(itemTotal);
        item.Mocktail.Should().Be(mocktail);
        item.Sale.Should().Be(sale);
    }

    [Fact]
    public void SaleProperty_ShouldUpdateSaleId()
    {
        // Arrange
        var item = new SaleItem();
        var sale = new Sale { Id = 15 };

        // Act
        item.Sale = sale;

        // Assert
        item.SaleId.Should().Be(15);
    }

    [Fact]
    public void MocktailProperty_ShouldUpdateMocktailId()
    {
        // Arrange
        var item = new SaleItem();
        var mocktail = new Mocktail { Id = 25 };

        // Act
        item.Mocktail = mocktail;

        // Assert
        item.MocktailId.Should().Be(25);
    }

    [Fact]
    public void SettingNullSale_ShouldSetSaleIdToZero()
    {
        // Arrange
        var item = new SaleItem { Sale = new Sale() };

        // Act
        item.Sale = null;

        // Assert
        item.SaleId.Should().Be(0);
    }

    [Fact]
    public void SettingNullMocktail_ShouldSetMocktailIdToZero()
    {
        // Arrange
        var item = new SaleItem { Mocktail = new Mocktail() };

        // Act
        item.Mocktail = null;

        // Assert
        item.MocktailId.Should().Be(0);
    }

    [Fact]
    public void TotalAmount_ShouldMirrorItemTotal()
    {
        // Arrange
        var item = new SaleItem { ItemTotal = 50.75m };

        // Act & Assert
        item.TotalAmount.Should().Be(50.75m);

        // Act
        item.TotalAmount = 100.25m;

        // Assert
        item.ItemTotal.Should().Be(100.25m);
    }

    [Fact]
    public void JsonIgnoreAttribute_ShouldPreventSaleSerialization()
    {
        // Arrange
        var item = new SaleItem 
        { 
            Id = 1,
            SaleId = 10, // Assign directly the ID
            MocktailId = 20,
            Mocktail = new Mocktail { Id = 20 } // Only needed if you want to test reference
        };

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // Act
        var json = JsonSerializer.Serialize(item, options);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json, options);

        // Assert
        json.Should().NotContain("Sale");
        json.Should().Contain("\"SaleId\":10");
        json.Should().Contain("\"MocktailId\":20");
    
        deserialized.Sale.Should().BeNull();
        deserialized.SaleId.Should().Be(10);
        deserialized.MocktailId.Should().Be(20);
    }

    [Fact]
    public void NotMappedAttribute_ShouldExcludeTotalAmountFromSerialization()
    {
        // Arrange
        var item = new SaleItem { TotalAmount = 150m };

        // Act
        var json = JsonSerializer.Serialize(item);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json);

        // Assert
        json.Should().NotContain("TotalAmount");
        deserialized.TotalAmount.Should().Be(0); // Non sérialisé
    }

    [Fact]
    public void SettingSaleId_ShouldNotAffectSaleReference()
    {
        // Arrange
        var item = new SaleItem { Sale = new Sale { Id = 10 } };

        // Act
        item.SaleId = 20;

        // Assert
        item.Sale.Id.Should().Be(10); // La référence existante ne change pas
    }

    [Fact]
    public void SettingMocktailId_ShouldNotAffectMocktailReference()
    {
        // Arrange
        var item = new SaleItem { Mocktail = new Mocktail { Id = 30 } };

        // Act
        item.MocktailId = 40;

        // Assert
        item.Mocktail.Id.Should().Be(30); // La référence existante ne change pas
    }
}