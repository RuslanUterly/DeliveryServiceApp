using DeliveryServiceApp.Models;
using DeliveryServiceApp.Repositories.Interfaces;
using DeliveryServiceApp.Validates.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeliveryServiceApp.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IOrderValidator orderValidator;
    private readonly ILogger<OrderRepository> logger;

    public OrderRepository(IOrderValidator orderValidator, ILogger<OrderRepository> logger)
    {
        this.orderValidator = orderValidator;
        this.logger = logger;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(string path)
    {
        if (!File.Exists(path))
        {
            logger.LogError($"Файл не найден: {path}");
            throw new FileNotFoundException($"Файл не найден: {path}");
        }

        var lines = File.ReadAllLines(path);
        var orders = new List<Order>();

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            if (await orderValidator.ValidateAsync(parts, out Order order))
            {
                orders.Add(order);
            }
        }

        logger.LogInformation($"Успешно прочитано заказов {orders.Count}");
        return orders;
    }

    public async Task SaveFilteredOrdersAsync(IEnumerable<Order> orders, string deliveryOrder)
    {
        try
        {
            using (var writer = new StreamWriter(deliveryOrder))
            {
                foreach (var order in orders)
                {
                    await writer.WriteLineAsync(order.ToString());
                }
            }
            logger.LogInformation($"Заказы сохранены в: {deliveryOrder}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Не удалось сохранить отфильтрованные заказы по пути: {deliveryOrder}");
        }
    }
}
