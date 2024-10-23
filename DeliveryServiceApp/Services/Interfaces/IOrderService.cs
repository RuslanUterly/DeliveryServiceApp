using DeliveryServiceApp.Models;

namespace DeliveryServiceApp.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> FilterOrdersAsync(IEnumerable<Order> orders, string district, DateTime firstDeliveryTime);
}
