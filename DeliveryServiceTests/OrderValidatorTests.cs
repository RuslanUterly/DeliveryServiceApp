using DeliveryServiceApp.Models;
using DeliveryServiceApp.Validates;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;

namespace DeliveryServiceTests;

public class OrderValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ValidInput()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderValidator>>();
        var validator = new OrderValidator(loggerMock.Object);
        string[] parts = new string[]
        {
            "123",
            "5,5", 
            "Коптево", 
            "2024-10-21 14:30:00" 
        };

        // Act
        var result = await validator.ValidateAsync(parts, out Order order);

        // Assert
        Assert.True(result);
        Assert.NotNull(order);
        Assert.Equal(123, order.Id);
        Assert.Equal(5.5, order.Weight);
        Assert.Equal("Коптево", order.Destrict);
        Assert.Equal(
            DateTime.ParseExact("2024-10-21 14:30:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            order.DeliveryTime);
    }

    [Fact]
    public async Task ValidateAsync_InvalidId()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderValidator>>();
        var validator = new OrderValidator(loggerMock.Object);
        string[] parts = new string[]
        {
            "abc", // некорректный id
            "5,5",
            "Коптево",
            "2024-10-21 14:30:00"
        };

        // Act
        var result = await validator.ValidateAsync(parts, out Order order);

        // Assert
        Assert.False(result);
        Assert.Null(order);
    }

    [Fact]
    public async Task ValidateAsync_InvalidDateFormat()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderValidator>>();
        var validator = new OrderValidator(loggerMock.Object);
        string[] parts = new string[]
        {
            "123",
            "5,5",
            "Коптево",
            "21-10-2024 14:30:00" // некорректный формат даты
        };

        // Act
        var result = await validator.ValidateAsync(parts, out Order order);

        // Assert
        Assert.False(result);
        Assert.Null(order);
    }
}
