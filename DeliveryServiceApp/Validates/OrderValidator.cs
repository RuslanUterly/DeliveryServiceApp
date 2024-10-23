using DeliveryServiceApp.Models;
using DeliveryServiceApp.Validates.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace DeliveryServiceApp.Validates;

public class OrderValidator(ILogger<OrderValidator> logger) : IOrderValidator
{
    public Task<bool> ValidateAsync(string[] parts, out Order order)
    {
        order = default!;

        if (parts.Length != 4)
        {
            logger.LogWarning($"Неверное количество полей в строке: {string.Join(';', parts)}");
            return Task.FromResult(false);
        }
        if (!int.TryParse(parts[0], out int id))
        {
            logger.LogWarning($"Неверный id: {parts[0]}");
            return Task.FromResult(false);
        }

        if (!double.TryParse(parts[1], out double weight))
        {
            logger.LogWarning($"Неверный вес заказа: {parts[1]}");
            return Task.FromResult(false);
        }

        if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryTime))
        {
            logger.LogWarning($"Неверный формат времени доставки: {parts[3]}");
            return Task.FromResult(false);
        }

        string address = parts[2];
        order = new Order(id, weight, address, deliveryTime);

        return Task.FromResult(true);
    }
}