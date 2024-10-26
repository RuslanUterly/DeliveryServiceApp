using DeliveryServiceApp.Models;
using DeliveryServiceApp.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryServiceTests;

public class OrderServiceTests
{
    [Fact]
    public async Task FilterOrdersAsync_OrdersMatchCriteria()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderService>>();
        var orderService = new OrderService(loggerMock.Object);

        var orders = new List<Order>
        {
            new Order(1, 26, "Коптево", DateTime.Parse("2024-10-21 10:00:00")),
            new Order(2, 37, "Хорошево", DateTime.Parse("2024-10-21 10:15:00")),
            new Order(3, 18, "Коптево", DateTime.Parse("2024-10-21 10:30:00")),
            new Order(4, 45, "Коптево", DateTime.Parse("2024-10-21 11:00:00"))
        };

        string district = "Коптево";
        DateTime firstDeliveryTime = DateTime.Parse("2024-10-21 10:00:00");

        // Act
        var result = await orderService.FilterOrdersAsync(orders, district, firstDeliveryTime);

        // Assert
        var expectedOrders = orders.Where(o => o.Id == 1 || o.Id == 3);
        Assert.Equal(expectedOrders, result);
    }

    [Fact]
    public async Task FilterOrdersAsync_NoOrdersMatchCriteria()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderService>>();
        var orderService = new OrderService(loggerMock.Object);

        var orders = new List<Order>
        {
            new Order(1, 26, "Бутово", DateTime.Parse("2024-10-21 09:00:00")),
            new Order(2, 37, "Хорошево", DateTime.Parse("2024-10-21 09:15:00"))
        };

        string district = "Коптево";
        DateTime firstDeliveryTime = DateTime.Parse("2024-10-21 10:00:00");

        // Act
        var result = await orderService.FilterOrdersAsync(orders, district, firstDeliveryTime);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task FilterOrdersAsync_OrderAtBoundaryTimes()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderService>>();
        var orderService = new OrderService(loggerMock.Object);

        var orders = new List<Order>
        {
            new Order(1, 26, "Коптево", DateTime.Parse("2024-10-21 10:00:00")), // Начало диапазона
            new Order(2, 37, "Коптево", DateTime.Parse("2024-10-21 10:30:00")), // Конец диапазона
            new Order(3, 18, "Коптево", DateTime.Parse("2024-10-21 10:31:00"))  // За пределами диапазона
        };

        string district = "Коптево";
        DateTime firstDeliveryTime = DateTime.Parse("2024-10-21 10:00:00");

        // Act
        var result = await orderService.FilterOrdersAsync(orders, district, firstDeliveryTime);

        // Assert
        var expectedOrders = orders.Where(o => o.Id == 1 || o.Id == 2);
        Assert.Equal(expectedOrders, result);
    }
}
