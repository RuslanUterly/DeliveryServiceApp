using DeliveryServiceApp.Models;

namespace DeliveryServiceApp.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetOrdersAsync(string path);
    public Task SaveFilteredOrdersAsync(IEnumerable<Order> orders, string deliveryOrder);
}
