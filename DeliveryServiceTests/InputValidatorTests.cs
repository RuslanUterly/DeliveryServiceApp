using DeliveryServiceApp.Validates;
using System.Globalization;

namespace DeliveryServiceTests;

public class InputValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ValidArgs()
    {
        // Arrange
        var validator = new InputValidator();
        string[] args =
        [
            "Коптево",
            "2024-10-21 23:23:23",
            "C:\\logs\\delivery.log",
            "C:\\orders\\filteredOrders.txt"
        ];

        // Act
        var result = await validator.ValidateAsync(args);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Коптево", result.District);
        Assert.Equal(
            DateTime.ParseExact("2024-10-21 23:23:23", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            result.FirstDeliveryTime);
        Assert.Equal("C:\\logs\\delivery.log", result.DeliveryLogPath);
        Assert.Equal("C:\\orders\\filteredOrders.txt", result.DeliveryOrderPath);
    }

    [Theory]
    [InlineData("Коптево", "2024-10-21 23:23:23", "C:\\logs\\delivery.log")] // 3 аргумента
    [InlineData("Коптево", "2024-10-21 23:23:23")] // 2 аргумента
    [InlineData("Коптево")] // 1 аргумент
    [InlineData()] // 0 аргументов
    [InlineData("Коптево", "2024-10-21 23:23:23", "C:\\logs\\delivery.log", "C:\\orders\\filteredOrders.txt", "ExtraArg")] // 5 аргументов
    public async Task ValidateAsync_InvalidArgsCount_ThrowsArgumentException(params string[] args)
    {
        // Arrange
        var validator = new InputValidator();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => validator.ValidateAsync(args));
        Assert.Contains("Неверное количество параметров", ex.Message);
    }

    [Fact]
    public async Task ValidateAsync_InvalidDateFormat()
    {
        // Arrange
        var validator = new InputValidator();
        string[] args = new string[]
        {
            "Коптево",
            "21-10-2024 23:23:23", // Неверный формат даты
            "C:\\logs\\delivery.log",
            "C:\\orders\\filteredOrders.txt"
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => validator.ValidateAsync(args));
        Assert.Contains("Неверный формат даты. Ожидается формат yyyy-MM-dd HH:mm:ss", ex.Message);
    }
}
