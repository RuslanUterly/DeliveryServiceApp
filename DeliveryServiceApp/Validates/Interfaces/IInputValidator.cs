using DeliveryServiceApp.Models;

namespace DeliveryServiceApp.Validates.Interfaces;

public interface IInputValidator
{
    Task<AppConfig> ValidateAsync(string[] args);
}
