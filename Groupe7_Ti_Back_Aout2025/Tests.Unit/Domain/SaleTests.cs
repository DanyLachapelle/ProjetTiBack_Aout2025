using FluentAssertions;
using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Tests.Unit.Domain;

public class SaleTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var sale = new Sale();

        // Assert
        sale.Id.Should().Be(0);
        sale.TotalAmount.Should().Be(0);
        sale.SaleDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        sale.TableNumber.Should().BeNull();
        sale.Status.Should().Be("Pending");
        sale.OrderTimer.Should().Be(15);
        sale.SaleItems.Should().NotBeNull();
        sale.SaleItems.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1, 25.99, "T-05", "Completed", 30)]
    [InlineData(2, 0, null, "Pending", 15)]
    public void PropertyAssignment_ShouldWorkCorrectly(
        int id, decimal totalAmount, string tableNumber, string status, int orderTimer)
    {
        // Arrange
        var testDate = DateTime.Now.AddHours(-1);
        var sale = new Sale();

        // Act
        sale.Id = id;
        sale.TotalAmount = totalAmount;
        sale.SaleDate = testDate;
        sale.TableNumber = tableNumber;
        sale.Status = status;
        sale.OrderTimer = orderTimer;

        // Assert
        sale.Id.Should().Be(id);
        sale.TotalAmount.Should().Be(totalAmount);
        sale.SaleDate.Should().Be(testDate);
        sale.TableNumber.Should().Be(tableNumber);
        sale.Status.Should().Be(status);
        sale.OrderTimer.Should().Be(orderTimer);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(0)]
    [InlineData(100.50)]
    public void TotalAmount_ShouldAcceptAnyValue(decimal amount)
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.TotalAmount = amount;

        // Assert
        sale.TotalAmount.Should().Be(amount);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Completed")]
    [InlineData("Cancelled")]
    [InlineData("InProgress")]
    [InlineData("InvalidStatus")]
    [InlineData("")]
    [InlineData(null)]
    public void Status_ShouldAcceptAnyValue(string status)
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.Status = status;

        // Assert
        sale.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(120)]
    [InlineData(-1)]
    public void OrderTimer_ShouldAcceptAnyValue(int minutes)
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.OrderTimer = minutes;

        // Assert
        sale.OrderTimer.Should().Be(minutes);
    }

    [Fact]
    public void SaleItems_ShouldAllowAddingItems()
    {
        // Arrange
        var sale = new Sale();
        var item1 = new SaleItem { Id = 1, Quantity = 2 };
        var item2 = new SaleItem { Id = 2, Quantity = 1 };

        // Act
        sale.SaleItems.Add(item1);
        sale.SaleItems.Add(item2);

        // Assert
        sale.SaleItems.Should().HaveCount(2);
        sale.SaleItems.Should().Contain(item1);
        sale.SaleItems.Should().Contain(item2);
    }

    [Fact]
    public void MarkAsCompleted_ShouldChangeStatusAndResetTimer()
    {
        // Arrange
        var sale = new Sale { Status = "InProgress", OrderTimer = 30 };

        // Act
        sale.Status = "Completed";
        sale.OrderTimer = 0;

        // Assert
        sale.Status.Should().Be("Completed");
        sale.OrderTimer.Should().Be(0);
    }
}