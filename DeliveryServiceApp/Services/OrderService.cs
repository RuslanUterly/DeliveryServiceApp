using DeliveryServiceApp.Models;
using DeliveryServiceApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DeliveryServiceApp.Services;

public class OrderService(ILogger<OrderService> logger) : IOrderService
{
    public Task<IEnumerable<Order>> FilterOrdersAsync(IEnumerable<Order> orders, string district, DateTime firstDeliveryTime) 
    {
        var endTime = firstDeliveryTime.AddMinutes(30);

        var filterOrders = orders
            .Where(o => o.Destrict.Equals(district, StringComparison.OrdinalIgnoreCase)
                     && o.DeliveryTime >= firstDeliveryTime
                     && o.DeliveryTime <= endTime);

        if (!filterOrders.Any())
            logger.LogError("В указаном диапазоне не найдено доставок");
        else
            logger.LogInformation($"Заказы отфильтрованы {filterOrders.ToArray().Length}");

        return Task.FromResult(filterOrders);
    }
}
