using DeliveryServiceApp.Models;

namespace DeliveryServiceApp.Validates.Interfaces;

public interface IOrderValidator
{
    Task<bool> ValidateAsync(string[] parts, out Order order);
}
