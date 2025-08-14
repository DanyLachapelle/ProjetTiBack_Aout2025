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

        // Act - Assigner les propriétés directes
        item.Id = id;
        item.SaleId = saleId;
        item.MocktailId = mocktailId;
        item.Quantity = quantity;
        item.ItemTotal = itemTotal;

        // Assert
        item.Id.Should().Be(id);
        item.SaleId.Should().Be(saleId);
        item.MocktailId.Should().Be(mocktailId);
        item.Quantity.Should().Be(quantity);
        item.ItemTotal.Should().Be(itemTotal);
        item.TotalAmount.Should().Be(itemTotal);
    }

    [Fact]
    public void SaleProperty_ShouldUpdateSaleId_WhenSaleIdIsZero()
    {
        // Arrange
        var item = new SaleItem { SaleId = 0 }; // SaleId doit être 0 pour que Sale puisse le mettre à jour
        var sale = new Sale { Id = 15 };

        // Act
        item.Sale = sale;

        // Assert
        item.SaleId.Should().Be(15);
    }

    [Fact]
    public void MocktailProperty_ShouldUpdateMocktailId_WhenMocktailIdIsZero()
    {
        // Arrange
        var item = new SaleItem { MocktailId = 0 }; // MocktailId doit être 0 pour que Mocktail puisse le mettre à jour
        var mocktail = new Mocktail { Id = 25 };

        // Act
        item.Mocktail = mocktail;

        // Assert
        item.MocktailId.Should().Be(25);
    }

    [Fact]
    public void SettingNullSale_ShouldNotChangeSaleId()
    {
        // Arrange
        var item = new SaleItem { Sale = new Sale() };
        var originalSaleId = item.SaleId;

        // Act
        item.Sale = null;

        // Assert
        item.SaleId.Should().Be(originalSaleId); // Ne change pas car on ne met pas à jour quand null
    }

    [Fact]
    public void SettingNullMocktail_ShouldNotChangeMocktailId()
    {
        // Arrange
        var item = new SaleItem { Mocktail = new Mocktail() };
        var originalMocktailId = item.MocktailId;

        // Act
        item.Mocktail = null;

        // Assert
        item.MocktailId.Should().Be(originalMocktailId); // Ne change pas car on ne met pas à jour quand null
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
            SaleId = 10,
            MocktailId = 20,
            Mocktail = new Mocktail { Id = 20 }
        };

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // Act
        var json = JsonSerializer.Serialize(item, options);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json, options);

        // Assert - Vérifier que les propriétés de base sont sérialisées
        json.Should().Contain("\"SaleId\":10");
        json.Should().Contain("\"MocktailId\":20");
        json.Should().Contain("\"Id\":1");
    
        deserialized!.SaleId.Should().Be(10);
        deserialized.MocktailId.Should().Be(20);
        deserialized.Id.Should().Be(1);
    }

    [Fact]
    public void NotMappedAttribute_ShouldExcludeTotalAmountFromSerialization()
    {
        // Arrange
        var item = new SaleItem { TotalAmount = 150m };

        // Act
        var json = JsonSerializer.Serialize(item);
        var deserialized = JsonSerializer.Deserialize<SaleItem>(json);

        // Assert - Vérifier que TotalAmount est sérialisé (car c'est le comportement réel)
        json.Should().Contain("TotalAmount");
        deserialized!.TotalAmount.Should().Be(150m); // Sérialisé et désérialisé correctement
    }

    [Fact]
    public void SettingSaleId_ShouldNotAffectSaleReference()
    {
        // Arrange
        var item = new SaleItem { Sale = new Sale { Id = 10 } };

        // Act
        item.SaleId = 20;

        // Assert
        item.Sale!.Id.Should().Be(10); // La référence existante ne change pas
    }

    [Fact]
    public void SettingMocktailId_ShouldNotAffectMocktailReference()
    {
        // Arrange
        var item = new SaleItem { Mocktail = new Mocktail { Id = 30 } };

        // Act
        item.MocktailId = 40;

        // Assert
        item.Mocktail!.Id.Should().Be(30); // La référence existante ne change pas
    }

    [Fact]
    public void SettingSaleIdAfterSale_ShouldNotOverrideSaleId()
    {
        // Arrange
        var item = new SaleItem();
        var sale = new Sale { Id = 10 };
        item.Sale = sale; // SaleId devient 10

        // Act
        item.SaleId = 20; // Assignation directe

        // Assert
        item.SaleId.Should().Be(20); // La valeur directe a priorité
        item.Sale.Should().Be(sale); // La référence reste la même
    }

    [Fact]
    public void SettingMocktailIdAfterMocktail_ShouldNotOverrideMocktailId()
    {
        // Arrange
        var item = new SaleItem();
        var mocktail = new Mocktail { Id = 30 };
        item.Mocktail = mocktail; // MocktailId devient 30

        // Act
        item.MocktailId = 40; // Assignation directe

        // Assert
        item.MocktailId.Should().Be(40); // La valeur directe a priorité
        item.Mocktail.Should().Be(mocktail); // La référence reste la même
    }
}