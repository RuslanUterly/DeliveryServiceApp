using DeliveryServiceApp.Models;
using DeliveryServiceApp.Repositories;
using DeliveryServiceApp.Validates.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryServiceTests;

public class OrderRepositoryTests
{
    [Fact]
    public async Task GetOrdersAsync_FileDoesNotExist()
    {
        // Arrange
        var orderValidatorMock = new Mock<IOrderValidator>();
        var loggerMock = new Mock<ILogger<OrderRepository>>();
        var repository = new OrderRepository(orderValidatorMock.Object, loggerMock.Object);
        string nonExistentFilePath = "nonexistentfile.txt";

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(async () =>
            await repository.GetOrdersAsync(nonExistentFilePath));
    }

    [Fact]
    public async Task GetOrdersAsync_ValidFile()
    {
        // Arrange
        var orderValidatorMock = new Mock<IOrderValidator>();
        var loggerMock = new Mock<ILogger<OrderRepository>>();
        var repository = new OrderRepository(orderValidatorMock.Object, loggerMock.Object);

        string tempFilePath = Path.GetTempFileName();
        try
        {
            // Создаем временный файл с данными заказов
            var fileContent = new[]
            {
                "1;26;Коптево;2024-10-21 23:23:23",
                "2;37;Хорошево;2024-10-22 23:23:23",
                "3;18;Бутово;2024-10-23 23:23:23"
            };
            await File.WriteAllLinesAsync(tempFilePath, fileContent);

            // Настраиваем заглушку валидатора
            orderValidatorMock
             .Setup(v => v.ValidateAsync(It.IsAny<string[]>(), out It.Ref<Order>.IsAny))
             .Returns((string[] parts, out Order order) =>
             {
                 order = new Order(int.Parse(parts[0]), double.Parse(parts[1]), parts[2], DateTime.Parse(parts[3]));
                 return Task.FromResult(true);
             });


            // Act
            var orders = await repository.GetOrdersAsync(tempFilePath);

            // Assert
            Assert.NotNull(orders);
            Assert.Equal(3, orders.ToArray().Length);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public async Task SaveFilteredOrdersAsync_ValidOrders()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderRepository>>();
        var orderValidatorMock = new Mock<IOrderValidator>();
        var repository = new OrderRepository(orderValidatorMock.Object, loggerMock.Object);

        var orders = new List<Order>
        {
            new Order(1, 26, "Коптево", DateTime.Parse("2024-10-21 23:23:23")),
            new Order(2, 37, "Хорошево", DateTime.Parse("2024-10-22 23:23:23"))
        };

        string tempFilePath = Path.GetTempFileName();
        try
        {
            // Act
            await repository.SaveFilteredOrdersAsync(orders, tempFilePath);

            // Assert
            var savedContent = await File.ReadAllLinesAsync(tempFilePath);
            Assert.Equal(2, savedContent.Length);
            Assert.Equal("1;26;Коптево;2024-10-21 23:23:23", savedContent[0]);
            Assert.Equal("2;37;Хорошево;2024-10-22 23:23:23", savedContent[1]);
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }
}